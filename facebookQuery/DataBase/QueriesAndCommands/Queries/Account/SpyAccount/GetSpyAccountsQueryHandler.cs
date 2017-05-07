using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountsQueryHandler : IQueryHandler<GetSpyAccountsQuery, List<SpyAccountModel>>
    {
        private readonly DataBaseContext _context;

        public GetSpyAccountsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<SpyAccountModel> Handle(GetSpyAccountsQuery query)
        {
            var models =
                _context.SpyAccounts.Include(model => model.Cookies)
                    .Where(model => !model.IsDeleted)
                    .OrderBy(model => model.Id)
                    .Skip(query.Count*query.Page)
                    .Take(query.Count)
                    .Select(model => new SpyAccountModel
                    {
                        Id = model.Id,
                        Login = model.Login,
                        Password = model.Password,
                        PageUrl = model.PageUrl,
                        UserId = model.FacebookId,
                        Cookie = new CookieModel
                        {
                            CookieString = model.Cookies.CookiesString
                        },
                        Proxy = model.Proxy,
                        ProxyLogin = model.ProxyLogin,
                        ProxyPassword = model.ProxyPassword,
                        AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                        FacebookId = model.FacebookId,
                        Name = model.Name,
                        ConformationIsFailed = model.ConformationIsFailed,
                        ProxyDataIsFailed = model.ProxyDataIsFailed
                    })
                    .ToList();

            return models;
        }
    }
}
