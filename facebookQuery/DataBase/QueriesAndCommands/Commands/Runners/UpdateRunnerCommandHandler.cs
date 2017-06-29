using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class UpdateRunnerCommandHandler : ICommandHandler<UpdateRunnerCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public UpdateRunnerCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(UpdateRunnerCommand command)
        {
            var runner = _context.Runners.FirstOrDefault(model => model.Id == command.RunnerId);
            if (runner == null)
            {
                return new VoidCommandResponse();
            }

            runner.IsAllowed = command.IsAllowed;

            _context.Runners.AddOrUpdate(runner);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
