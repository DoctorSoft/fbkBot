using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountsByGroupSettingsIdQueryHandler : IQueryHandler<GetAccountsByGroupSettingsIdQuery, List<AccountModel>>
    {
        private readonly DataBaseContext _context;

        public GetAccountsByGroupSettingsIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<AccountModel> Handle(GetAccountsByGroupSettingsIdQuery query)
        {
            var models = 
                _context.Accounts.Include(model => model.Cookies)
                .Where(model => model.GroupSettingsId == query.GroupSettingsId)
                .Where(model => !model.AuthorizationDataIsFailed)
                .Where(model => !model.ProxyDataIsFailed)
                .Where(model => !model.ConformationIsFailed)
                .Where(model => !model.IsDeleted)
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
                    ConformationIsFailed = model.ConformationIsFailed,
                    UserAgentId = model.UserAgentId
                }).ToList();

            return models;
        }
    }
}
