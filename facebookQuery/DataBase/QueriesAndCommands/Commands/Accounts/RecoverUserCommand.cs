namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class RecoverUserCommand : IVoidCommand
    {
        public long AccountId { get; set; }
    }
}
