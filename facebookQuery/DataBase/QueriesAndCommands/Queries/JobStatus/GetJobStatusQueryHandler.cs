using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using CommonModels;
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
            var jobStatusList = _context.JobStatus.Where(model => model.FunctionName == command.FunctionName 
                && model.AccountId == command.AccountId
                && model.IsForSpy == command.IsForSpy 
                && model.FriendId == command.FriendId).ToList();

            if (jobStatusList.Count == 0)
            {
                return null;
            }

            var result = new List<JobStatusModel>();

            var jsSerializator = new JavaScriptSerializer();

            foreach (var jobStatusDbModel in jobStatusList)
            {
                var launchTimeModel = jsSerializator.Deserialize<TimeModel>(jobStatusDbModel.LaunchDateTime);

                result.Add(new JobStatusModel
                {
                    AccountId = jobStatusDbModel.AccountId,
                    Id = jobStatusDbModel.Id,
                    FunctionName = jobStatusDbModel.FunctionName,
                    AddDateTime = jobStatusDbModel.AddDateTime,
                    LaunchTime = launchTimeModel,
                    JobId = jobStatusDbModel.JobId,
                    FriendId = jobStatusDbModel.FriendId,
                    IsForSpy = jobStatusDbModel.IsForSpy
                });
            }

            return result;
        }
    }
}
