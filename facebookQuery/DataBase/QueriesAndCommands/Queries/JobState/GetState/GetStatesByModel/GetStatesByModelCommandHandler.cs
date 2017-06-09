using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobState.GetState.GetStatesByModel
{
    public class GetStatesByModelCommandHandler : ICommandHandler<GetStatesByModelCommand, List<JobStateModel>>
    {
        private readonly DataBaseContext _context;

        public GetStatesByModelCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public List<JobStateModel> Handle(GetStatesByModelCommand command)
        {
            var states = _context.JobsState
                .Where(model => model.AccountId == command.AccountId)
                .Where(model => model.IsForSpy == command.IsForSpy)
                .OrderByDescending(model => model.AddedDateTime);

            List<JobStateModel> result;

            if (command.FunctionName != null)
            {
                result = states.Where(model => model.FunctionName == command.FunctionName)
                    .Select(model => new JobStateModel
                    {
                        AccountId = model.AccountId,
                        Id = model.Id,
                        AddedDateTime = model.AddedDateTime,
                        FunctionName = model.FunctionName,
                        FriendId = model.FriendId,
                        IsForSpy = model.IsForSpy,
                        JobId = model.JobId
                    }).ToList();
            }
            else
            {
                result = states
                .Select(model => new JobStateModel
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    AddedDateTime = model.AddedDateTime,
                    FunctionName = model.FunctionName,
                    FriendId = model.FriendId,
                    IsForSpy = model.IsForSpy,
                    JobId = model.JobId
                }).ToList();
            }

            return result;
        }
    }
}
