namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class DeleteSpyAccounCommand : IVoidCommand
    {
        public long AccountId { get; set; }
    }
}
