namespace DataBase.QueriesAndCommands.Queries.AccountInformation
{
    public class GetAccountInformationQuery : IQuery<AccountInformation>
    {
        public long AccountId { get; set; }
    }
}
