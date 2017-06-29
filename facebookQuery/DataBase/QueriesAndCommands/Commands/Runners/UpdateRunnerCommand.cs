namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class UpdateRunnerCommand : IVoidCommand
    {
        public long RunnerId { get; set; }

        public bool IsAllowed { get; set; }
    }
}
