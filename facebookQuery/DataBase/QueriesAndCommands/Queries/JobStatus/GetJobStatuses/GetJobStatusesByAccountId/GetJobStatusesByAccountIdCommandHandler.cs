using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccountId
{
    public class GetJobStatusesByAccountIdCommandHandler : ICommandHandler<GetJobStatusesByAccountIdCommand, List<JobStatusModel>>
    {
        private readonly DataBaseContext _context;

        public GetJobStatusesByAccountIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public List<JobStatusModel> Handle(GetJobStatusesByAccountIdCommand command)
        {
            var statusesDbModels = _context.JobStatus
                .Where(model => model.AccountId == command.AccountId)
                .Where(model => model.IsForSpy == command.IsForSpy).ToList();

            var statuses = statusesDbModels.Select(model => new JobStatusModel
            {
                AccountId = model.AccountId,
                AddDateTime = model.AddDateTime,
                FriendId = model.FriendId,
                FunctionName = model.FunctionName,
                Id = model.Id,
                IsForSpy = model.IsForSpy,
                JobId = model.JobId
            }).ToList();

            return statuses;
        }
    }
}
