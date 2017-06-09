using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobState.GetState.GetStatesByModel
{
    public class GetStatesByModelCommand : ICommand<List<JobStateModel>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName? FunctionName { get; set; }
    }
}
