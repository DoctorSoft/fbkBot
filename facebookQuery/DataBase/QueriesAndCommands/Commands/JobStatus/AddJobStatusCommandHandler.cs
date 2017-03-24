using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class AddJobStatusCommandHandler : ICommandHandler<AddJobStatusCommand, long>
    {
        private readonly DataBaseContext _context;

        public AddJobStatusCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public long Handle(AddJobStatusCommand command)
        {
            var jobStatus = _context.JobStatus.FirstOrDefault(model => model.FunctionName == command.FunctionName 
                            && model.AccountId == command.AccountId && model.FriendId == command.FriendId) ??
                            new JobStatusDbModel
                            {
                                AccountId = command.AccountId,
                                FunctionName = command.FunctionName,
                                AddDateTime = DateTime.Now,
                                LaunchDateTime = command.LaunchDateTime,
                                JobId = command.JobId,
                                FriendId = command.FriendId
                            };
            
            _context.JobStatus.AddOrUpdate(jobStatus);

            _context.SaveChanges();

            return jobStatus.Id;
        }
    }
}
