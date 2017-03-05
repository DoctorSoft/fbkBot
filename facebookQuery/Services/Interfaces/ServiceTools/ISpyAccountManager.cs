using System.Net;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Interfaces.ServiceTools
{
    public interface ISpyAccountManager
    {
        SpyAccountModel GetSpyAccountById(long? spyAccountId);
        
        WebProxy GetSpyAccountProxy(SpyAccountModel spyAccount);

        SpyAccountModel GetSpyAccountByFacebookId(long spyAccountFacebookId);
    }
}
