using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, AccountModel>
    {
        private readonly DataBaseContext context;

        public GetAccountByIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public AccountModel Handle(GetAccountByIdQuery query)
        {
            var models = 
                context.Accounts.Include(model => model.Cookies)
                .Where(model => model.Id == query.UserId)
                .Select(model => new AccountModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    UserId = model.FacebookId,
                    Login = model.Login,
                    Password = model.Password,
                    Cookie = new CookieModel
                    {
                       CookieString = model.Cookies.CookiesString,
                       CreateDateTime = model.Cookies.CreateDate
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
