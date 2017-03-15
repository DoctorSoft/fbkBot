using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQueryHandler : IQueryHandler<GetJobStatusQuery, JobStatusModel>
    {
        private readonly DataBaseContext _context;

        public GetJobStatusQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public JobStatusModel Handle(GetJobStatusQuery command)
        {
            var jobStatus = _context.JobStatus.FirstOrDefault(model => model.FunctionName == command.FunctionName && model.AccountId == command.AccountId);

            if (jobStatus == null)
            {
                return null;
            }

            return new JobStatusModel
            {
                AccountId = jobStatus.AccountId,
                Id = jobStatus.Id,
                FunctionName = jobStatus.FunctionName,
                AddDateTime = jobStatus.AddDateTime,
                LaunchDateTime = jobStatus.LaunchDateTime,
                JobId = jobStatus.JobId
            };
        }
    }
}
