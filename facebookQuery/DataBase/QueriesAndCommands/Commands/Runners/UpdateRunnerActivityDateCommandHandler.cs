using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class UpdateRunnerActivityDateCommandHandler : ICommandHandler<UpdateRunnerActivityDateCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public UpdateRunnerActivityDateCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(UpdateRunnerActivityDateCommand command)
        {
            var runner = _context.Runners.FirstOrDefault(model => model.Id == command.RunnerId);
            if (runner == null)
            {
                return new VoidCommandResponse();
            }

            runner.LastAction = DateTime.Now;

            _context.Runners.AddOrUpdate(runner);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
