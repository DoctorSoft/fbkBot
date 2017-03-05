using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue
{
    public class AddToQueueCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
