using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetDeletedAccountsQueryHandler: IQueryHandler<GetDeletedAccountsQuery, List<AccountModel>>
    {
        private readonly DataBaseContext context;

        public GetDeletedAccountsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AccountModel> Handle(GetDeletedAccountsQuery query)
        {
            var models =
                context.Accounts.Include(model => model.Cookies)
                    .Where(model => model.IsDeleted)
                    .OrderBy(model => model.Id)
                    .Skip(query.Count*query.Page)
                    .Take(query.Count)
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
                        ProxyPassword = model.ProxyPassword
                    })
                    .ToList();

            return models;
        }
    }
}
