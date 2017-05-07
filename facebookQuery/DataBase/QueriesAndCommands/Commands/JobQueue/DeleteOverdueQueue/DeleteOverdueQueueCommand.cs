namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteOverdueQueue
{
    public class DeleteOverdueQueueCommand : ICommand<DeleteOverdueQueueResponseModel>
    {
        public int OverdueMin { get; set; }
    }
}
