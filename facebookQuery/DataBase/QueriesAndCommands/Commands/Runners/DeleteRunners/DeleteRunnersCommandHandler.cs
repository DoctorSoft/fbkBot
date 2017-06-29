using System;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Runners.DeleteRunners
{
    public class DeleteRunnersCommandHandler : ICommandHandler<DeleteRunnersCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteRunnersCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(DeleteRunnersCommand command)
        {
            var overdueMin = command.OverdueMin;

            var runnerModels = _context.Runners
                .Where(model => model.IsAllowed)
                .Select(model => new 
            {
                model.Id,
                model.CreateDate,
                model.DeviceName,
                model.IsAllowed,
                model.LastAction
            }).AsEnumerable().Select(model => new RunnerDbModel
            {
                Id = model.Id,
                CreateDate = model.CreateDate,
                DeviceName = model.DeviceName,
                IsAllowed = model.IsAllowed,
                LastAction = model.LastAction
            }).ToList();
            
            foreach (var runnerModel in runnerModels)
            {
                var isFailed = CheckOverdue(runnerModel.LastAction) > overdueMin;
                if (!isFailed)
                {
                    continue;
                }

                var runner = _context.Runners.Single(o => o.Id == runnerModel.Id);
                _context.Runners.Remove(runner);
                _context.SaveChanges();
            }

            
            return new VoidCommandResponse();
        }

        public long CheckOverdue(DateTime addedDateTime)
        {
            var differenceTime = DateTime.Now - addedDateTime;

            var differenceMin = differenceTime.Hours*60 + differenceTime.Minutes;

            return differenceMin;
        }
    }
}
