using System.Collections.Generic;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetDeletedAccountsQuery : IQuery<List<AccountModel>>
    {
        public int Count { get; set; }

        public int Page { get; set; }
    }
}
