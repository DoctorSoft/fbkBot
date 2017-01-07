using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public partial class GetAccountByFacebookIdQuery: IQuery<AccountModel>
    {
        public long FacebookUserId { get; set; }
    }
}
