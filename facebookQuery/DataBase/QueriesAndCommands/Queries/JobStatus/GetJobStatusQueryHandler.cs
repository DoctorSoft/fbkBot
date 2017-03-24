using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQueryHandler : IQueryHandler<GetJobStatusQuery, List<JobStatusModel>>
    {
        private readonly DataBaseContext _context;

        public GetJobStatusQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<JobStatusModel> Handle(GetJobStatusQuery command)
        {
            var jobStatusList = _context.JobStatus.Where(model => model.FunctionName == command.FunctionName && model.AccountId == command.AccountId && model.FriendId == command.FriendId).ToList();

            if (jobStatusList.Count == 0)
            {
                return null;
            }

            var result = new List<JobStatusModel>();
            
            foreach (var jobStatusDbModel in jobStatusList)
            {
                result.Add(new JobStatusModel
                {
                    AccountId = jobStatusDbModel.AccountId,
                    Id = jobStatusDbModel.Id,
                    FunctionName = jobStatusDbModel.FunctionName,
                    AddDateTime = jobStatusDbModel.AddDateTime,
                    LaunchDateTime = jobStatusDbModel.LaunchDateTime,
                    JobId = jobStatusDbModel.JobId,
                    FriendId = jobStatusDbModel.FriendId
                });
            }

            return result;
        }
    }
}
