using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobState.DeleteState
{
    public class DeleteJobStateCommandHandler : ICommandHandler<DeleteJobStateCommand, List<string>>
    {
        private readonly DataBaseContext _context;

        public DeleteJobStateCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public List<string> Handle(DeleteJobStateCommand command)
        {
            var jobIdList = new List<string>();
            try
            {
                var stateModels = _context.JobsState
                    .Where(model => model.AccountId == command.AccountId)
                    .Where(model => model.FriendId == command.FriendId)
                    .Where(model => model.FunctionName == command.FunctionName)
                    .Where(model => model.IsForSpy == command.IsForSpy).ToList();

                jobIdList = stateModels.Select(model => model.JobId).ToList();

                _context.JobsState.RemoveRange(stateModels);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }

            return jobIdList;
        }
    }
}
