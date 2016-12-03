using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountByFacebookIdQueryHandler : IQueryHandler<GetAccountByFacebookIdQuery, AccountModel>
    {
        private readonly DataBaseContext context;

        public GetAccountByFacebookIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public AccountModel Handle(GetAccountByFacebookIdQuery query)
        {
            var models = 
                context.Accounts.Include(model => model.Cookies)
                .Where(model => model.FacebookId == query.UserId)
                .Where(model => !model.IsDeleted)
                .Select(model => new AccountModel
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
