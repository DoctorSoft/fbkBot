using System.Net;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface ISpyAccountManager
    {
        SpyAccountModel GetSpyAccountById(long? spyAccountId);
        
        WebProxy GetSpyAccountProxy(SpyAccountModel spyAccount);

        SpyAccountModel GetSpyAccountByFacebookId(long spyAccountFacebookId);
    }
}
