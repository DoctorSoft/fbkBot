using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobState.GetState
{
    public class JobStateModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long? FriendId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }

        public string JobId { get; set; }
    }
}
