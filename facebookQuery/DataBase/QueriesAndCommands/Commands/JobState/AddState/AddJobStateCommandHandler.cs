using System;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.JobState.AddState
{
    public class AddJobStateCommandHandler : ICommandHandler<AddJobStateCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddJobStateCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(AddJobStateCommand command)
        {
            var stateModel = new JobStateDbModel
            {
                AccountId = command.AccountId,
                FunctionName = command.FunctionName,
                AddedDateTime = DateTime.Now,
                FriendId = command.FriendId,
                IsForSpy = command.IsForSpy,
                JobId = command.JobId
            };

            _context.JobsState.Add(stateModel);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
