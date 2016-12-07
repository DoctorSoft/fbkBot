using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetDeletedAccountsQuery : IQuery<List<AccountModel>>
    {
        public int Count { get; set; }

        public int Page { get; set; }
    }
}
