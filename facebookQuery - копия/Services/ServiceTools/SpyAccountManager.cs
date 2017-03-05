using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Account.SpyAccount;
using Services.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class SpyAccountManager : ISpyAccountManager
    {
        public SpyAccountModel GetSpyAccountById(long? spyAccountId)
        {
            return new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
            {
                UserId = spyAccountId
            });
        }

        public WebProxy GetSpyAccountProxy(SpyAccountModel spyAccount)
        {
            return new WebProxy(spyAccount.Proxy)
            {
                Credentials = new NetworkCredential(spyAccount.ProxyLogin, spyAccount.ProxyPassword)
            };
        }

        public SpyAccountModel GetSpyAccountByFacebookId(long spyAccountFacebookId)
        {
            return new GetSpyAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByFacebookIdQuery()
            {
                UserId = spyAccountFacebookId
            });
        }
    }
}
