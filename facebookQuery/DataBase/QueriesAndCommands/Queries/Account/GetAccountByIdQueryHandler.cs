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
                    Cookie = new CookieModel
                    {
                        Act = model.Cookies.Act,
                        Av = model.Cookies.Av,
                        CUser = model.Cookies.CUser,
                        Csm = model.Cookies.Csm,
                        Datr = model.Cookies.Datr,
                        Fr = model.Cookies.Fr,
                        Locale = model.Cookies.Locale,
                        Lu = model.Cookies.Lu,
                        P = model.Cookies.P,
                        Pl = model.Cookies.Pl,
                        Presence = model.Cookies.Presence,
                        S = model.Cookies.S,
                        Sb = model.Cookies.Sb,
                        Wd = model.Cookies.Wd,
                        Xs = model.Cookies.Xs
                    }
                }).FirstOrDefault();

            return models;
        }
    }
}
