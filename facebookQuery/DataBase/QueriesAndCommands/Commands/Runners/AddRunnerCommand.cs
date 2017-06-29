namespace DataBase.QueriesAndCommands.Commands.Runners
{
    public class AddRunnerCommand : ICommand<long>
    {
        public string DeviceName{ get; set; }

        public bool IsAllowed { get; set; }
    }
}
