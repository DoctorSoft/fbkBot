namespace DataBase.QueriesAndCommands.Commands.Runners.DeleteRunners
{
    public class DeleteRunnersCommand : IVoidCommand
    {
        public int OverdueMin { get; set; }
    }
}
