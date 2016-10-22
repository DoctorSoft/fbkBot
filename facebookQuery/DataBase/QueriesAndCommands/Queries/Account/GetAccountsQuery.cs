using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountsQuery : IQuery<List<AccountModel>>
    {
        public int Count { get; set; }

        public int Page { get; set; }
    }
}
