using System.Data.Entity;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace DataBase.QueriesAndCommands.Queries.Account
{
    public class GetAccountByFacebookIdQueryHandler : IQueryHandler<GetAccountByFacebookIdQuery, AccountModel>
    {
        private readonly DataBaseContext _context;

        public GetAccountByFacebookIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public AccountModel Handle(GetAccountByFacebookIdQuery query)
        {
            var models = 
                _context.Accounts.Include(model => model.Cookies)
                .Where(model => model.FacebookId == query.FacebookUserId)
                .Where(model => !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId))
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
                    ConformationIsFailed = model.ConformationIsFailed
                }).FirstOrDefault();

            return models;
        }
    }
}
