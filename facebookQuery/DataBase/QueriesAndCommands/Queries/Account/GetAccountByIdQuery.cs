using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountByIdQuery: IQuery<AccountModel>
    {
        public long UserId { get; set; }
    }
}
