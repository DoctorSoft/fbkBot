using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobState.JobStateIsExist
{
    public class JobStateIsExistQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }
    }
}
