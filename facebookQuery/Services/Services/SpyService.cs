using System.Threading;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Friends;
using Engines.Engines.GetFriendInfoEngine;
using Services.Core;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class SpyService
    {        
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;

        public SpyService()
        {
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
        }
        public void AnalyzeFriends(AccountViewModel accountViewModel)
        {
            var account = new AccountModel()
            {
                Id = accountViewModel.Id,
                FacebookId = accountViewModel.FacebookId,
                Login = accountViewModel.Login,
                Name = accountViewModel.Name,
                PageUrl = accountViewModel.PageUrl,
                Password = accountViewModel.Password,
                Proxy = accountViewModel.Proxy,
                ProxyLogin = accountViewModel.ProxyLogin,
                ProxyPassword = accountViewModel.ProxyPassword,
                UserId = accountViewModel.Id,
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                }
            };

            var friendList = new GetAnalisysFriendsQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsQuery());
            var settings = _accountSettingsManager.GetAccountSettings(accountViewModel.Id);

            foreach (var analysisFriendData in friendList)
            {
                var friendInfo = new GetFriendInfoEngine().Execute(new GetFriendInfoModel()
                {
                    AccountFacebookId = account.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie.CookieString,
                    FriendFacebookId = analysisFriendData.FacebookId,
                    Settings = settings
                });


                new AnalizeFriendCore().StartAnalyze(settings, new FriendInfoViewModel
                {
                    FacebookId = analysisFriendData.FacebookId,
                    Gender = friendInfo.Gender,
                    LivesSection = friendInfo.LivesSection,
                    RelationsSection = friendInfo.RelationsSection,
                    SchoolSection = friendInfo.SchoolSection,
                    WorkSection = friendInfo.WorkSection
                });

                Thread.Sleep(5000);
            }
        }
    }
}
