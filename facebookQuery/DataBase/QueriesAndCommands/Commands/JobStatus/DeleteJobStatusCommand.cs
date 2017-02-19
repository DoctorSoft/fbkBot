namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class DeleteJobStatusCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }
    }
}
