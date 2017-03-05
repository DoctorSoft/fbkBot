using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, List<AccountModel>>
    {
        private readonly DataBaseContext context;

        public GetAccountsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AccountModel> Handle(GetAccountsQuery query)
        {
            var models =
                context.Accounts.Include(model => model.Cookies)
                    .Where(model => !model.IsDeleted)
                    .OrderBy(model => model.Id)
                    //.Skip(query.Count*query.Page)
                    //.Take(query.Count)
                    .Select(model => new AccountModel
                    {
                        Id = model.Id,
                        Login = model.Login,
                        Password = model.Password,
                        PageUrl = model.PageUrl,
                        FacebookId = model.FacebookId,
                        Cookie = new CookieModel
                        {
                            CookieString = model.Cookies.CookiesString
                        },
                        Proxy = model.Proxy,
                        ProxyLogin = model.ProxyLogin,
                        ProxyPassword = model.ProxyPassword,
                        GroupSettingsId = model.GroupSettingsId,
                        Name = model.Name,
                        AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                        ProxyDataIsFailed = model.ProxyDataIsFailed,
                        IsDeleted = model.IsDeleted
                    })
                    .ToList();

            return models;
        }
    }
}
