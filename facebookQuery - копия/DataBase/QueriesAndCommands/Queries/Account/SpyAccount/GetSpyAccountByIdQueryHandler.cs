using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account.SpyAccount
{
    public class GetSpyAccountByIdQueryHandler : IQueryHandler<GetSpyAccountByIdQuery, SpyAccountModel>
    {
        private readonly DataBaseContext context;

        public GetSpyAccountByIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public SpyAccountModel Handle(GetSpyAccountByIdQuery query)
        {
            var spyModel =
                context.SpyAccounts.Include(model => model.Cookies)
                    .Where(model => model.Id == query.UserId)
                    .Select(model => new SpyAccountModel
                    {
                        Id = model.Id,
                        PageUrl = model.PageUrl,
                        UserId = model.FacebookId,
                        Login = model.Login,
                        Password = model.Password,
                        Cookie = model.Cookies!=null ? new CookieModel
                        {
                            CookieString = model.Cookies.CookiesString,
                            CreateDateTime = model.Cookies.CreateDate
                        } : null,
                        Name = model.Name,
                        FacebookId = model.FacebookId,
                        Proxy = model.Proxy,
                        ProxyLogin = model.ProxyLogin,
                        ProxyPassword = model.ProxyPassword
                    }).FirstOrDefault();

            return spyModel;
        }
    }
}
