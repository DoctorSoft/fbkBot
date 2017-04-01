using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.GetWorkAccounts
{
    public class GetWorkAccountsQueryHandler : IQueryHandler<GetWorkAccountsQuery, List<AccountModel>>
    {
        private readonly DataBaseContext _context;

        public GetWorkAccountsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<AccountModel> Handle(GetWorkAccountsQuery query)
        {
            var models = 
                _context.Accounts.Include(model => model.Cookies)
                .Where(model => !model.AuthorizationDataIsFailed && !model.IsDeleted && !model.ProxyDataIsFailed && !model.ConformationIsFailed)
                .Select(model => new AccountModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    FacebookId = model.FacebookId,
                    Login = model.Login,
                    Password = model.Password,
                    Cookie = new CookieModel
                    {
                       CookieString = model.Cookies.CookiesString
                    },
                    Name = model.Name,
                    Proxy = model.Proxy,
                    ProxyLogin = model.ProxyLogin,
                    ProxyPassword = model.ProxyPassword,
                    GroupSettingsId = model.GroupSettingsId,
                    AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                    ProxyDataIsFailed = model.ProxyDataIsFailed,
                    ConformationIsFailed = model.ConformationIsFailed
                }).ToList();

            return models;
        }
    }
}
