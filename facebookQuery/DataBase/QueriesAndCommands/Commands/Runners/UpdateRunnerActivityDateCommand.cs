namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class UpdateRunnerActivityDateCommand : IVoidCommand
    {
        public long RunnerId { get; set; }
    }
}
