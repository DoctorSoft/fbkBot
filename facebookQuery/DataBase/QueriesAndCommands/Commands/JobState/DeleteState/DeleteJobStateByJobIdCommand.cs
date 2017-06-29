namespace DataBase.QueriesAndCommands.Commands.JobState.DeleteState
{
    public class DeleteJobStateByJobIdCommand : IVoidCommand
    {
        public string JobId { get; set; }
    }
}
