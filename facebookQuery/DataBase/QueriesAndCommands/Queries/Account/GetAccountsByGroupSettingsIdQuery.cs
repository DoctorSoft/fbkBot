using System.Collections.Generic;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountsByGroupSettingsIdQuery : IQuery<List<AccountModel>>
    {
        public long GroupSettingsId { get; set; }
    }
}
