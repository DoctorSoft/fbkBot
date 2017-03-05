using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountByFacebookIdQuery : IQuery<SpyAccountModel>
    {
        public long UserId { get; set; }
    }
}
