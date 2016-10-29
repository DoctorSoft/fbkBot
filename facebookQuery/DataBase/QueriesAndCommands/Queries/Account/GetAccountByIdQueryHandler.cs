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
                .Where(model => model.UserId == query.UserId)
                .Select(model => new AccountModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    UserId = model.UserId,
                    Login = model.Login,
                    Password = model.Password,
                    Cookie = new CookieModel
                    {
                       CookieString = model.Cookies.CookiesString
                    }
                }).FirstOrDefault();

            return models;
        }
    }
}
