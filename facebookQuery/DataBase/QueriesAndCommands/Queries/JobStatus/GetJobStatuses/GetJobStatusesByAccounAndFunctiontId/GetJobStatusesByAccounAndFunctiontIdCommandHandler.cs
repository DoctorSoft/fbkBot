using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccounAndFunctiontId
{
    public class GetJobStatusesByAccounAndFunctiontIdCommandHandler : ICommandHandler<GetJobStatusesByAccounAndFunctiontIdCommand, List<JobStatusModel>>
    {
        private readonly DataBaseContext _context;

        public GetJobStatusesByAccounAndFunctiontIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public List<JobStatusModel> Handle(GetJobStatusesByAccounAndFunctiontIdCommand command)
        {
            var statusesDbModels = _context.JobStatus
                .Where(model => model.AccountId == command.AccountId)
                .Where(model => model.FunctionName == command.FunctionName)
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
