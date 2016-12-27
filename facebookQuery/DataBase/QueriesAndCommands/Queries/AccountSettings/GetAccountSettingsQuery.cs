using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.AccountSettings
{
    public class GetAccountSettingsQuery: IQuery<AccountSettingsModel>
    {
        public long AccountId { get; set; }
    }
}
