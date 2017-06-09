using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.JobQueuIsExist
{
    public class JobQueuIsExistQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }
    }
}
