namespace DataBase.QueriesAndCommands.Queries.AccountSettings
{
    public class GetAccountSettingsQuery : IQuery<AccountOptionsData>
    {
        public long AccountId { get; set; }
    }
}
