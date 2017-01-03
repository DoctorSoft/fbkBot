using System.Collections.Generic;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountsQuery : IQuery<List<SpyAccountModel>>
    {
        public int Count { get; set; }

        public int Page { get; set; }
    }
}
