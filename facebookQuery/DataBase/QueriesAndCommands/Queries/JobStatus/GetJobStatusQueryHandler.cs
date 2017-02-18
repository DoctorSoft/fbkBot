using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQueryHandler : IQueryHandler<GetJobStatusQuery, JobStatusData>
    {
        private readonly DataBaseContext _context;

        public GetJobStatusQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public JobStatusData Handle(GetJobStatusQuery command)
        {
            var jobStatus = _context.JobStatus.FirstOrDefault(model => model.FunctionName == command.FunctionName);

            if (jobStatus == null)
            {
                return null;
            }

            return new JobStatusData
            {
                LastLaunchDateTime = jobStatus.LastLaunchDateTime,
                Id = jobStatus.Id,
                FunctionName = jobStatus.FunctionName
            };
        }
    }
}
