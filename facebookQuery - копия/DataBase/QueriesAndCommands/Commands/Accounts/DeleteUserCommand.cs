namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class DeleteUserCommand : IVoidCommand
    {
        public long AccountId { get; set; }
    }
}
