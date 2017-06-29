using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.JobQueueIsExist
{
    public class JobQueueIsExistQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }
    }
}
