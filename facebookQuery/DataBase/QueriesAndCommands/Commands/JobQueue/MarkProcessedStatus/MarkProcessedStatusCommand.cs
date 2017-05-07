using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.MarkProcessedStatus
{
    public class MarkProcessedStatusCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
