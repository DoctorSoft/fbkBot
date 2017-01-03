using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountByFacebookIdQueryHandler : IQueryHandler<GetSpyAccountByFacebookIdQuery, SpyAccountModel>
    {
        private readonly DataBaseContext context;

        public GetSpyAccountByFacebookIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public SpyAccountModel Handle(GetSpyAccountByFacebookIdQuery query)
        {
            var models = 
                context.SpyAccounts.Include(model => model.Cookies)
                .Where(model => model.FacebookId == query.UserId)
                .Where(model => !model.IsDeleted)
                .Select(model => new SpyAccountModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    UserId = model.FacebookId,
                    Login = model.Login,
                    Password = model.Password,
                    Cookie = new CookieModel
                    {
                       CookieString = model.Cookies.CookiesString
                    },
                    Name = model.Name,
                    FacebookId = model.FacebookId,
                    Proxy = model.Proxy,
                    ProxyLogin = model.ProxyLogin,
                    ProxyPassword = model.ProxyPassword
                }).FirstOrDefault();

            return models;
        }
    }
}
