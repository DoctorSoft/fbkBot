using System;
using System.Data.Entity.Migrations;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class AddRunnerCommandHandler : ICommandHandler<AddRunnerCommand, long>
    {
        private readonly DataBaseContext _context;

        public AddRunnerCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public long Handle(AddRunnerCommand command)
        {
            var runner = new RunnerDbModel()
            {
                CreateDate = DateTime.Now,
                IsAllowed = true,
                DeviceName = command.DeviceName,
                LastAction = DateTime.Now
            };
            _context.Runners.AddOrUpdate(runner);

            _context.SaveChanges();

            return runner.Id;
        }
    }
}
