using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountByIdQuery: IQuery<SpyAccountModel>
    {
        public long? UserId { get; set; }
    }
}
