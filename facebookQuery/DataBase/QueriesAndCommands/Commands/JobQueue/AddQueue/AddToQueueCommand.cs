using CommonModels;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue
{
    public class AddToQueueCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public bool IsUnique { get; set; }
        
        public string JobId { get; set; }

        public TimeModel LaunchDateTime { get; set; }
    }
}
