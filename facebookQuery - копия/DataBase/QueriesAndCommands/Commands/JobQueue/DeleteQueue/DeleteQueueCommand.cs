namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueue
{
    public class DeleteQueueCommand : ICommand<VoidCommandResponse>
    {
        public long QueueId { get; set; }
    }
}
