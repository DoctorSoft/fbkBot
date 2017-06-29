using System.Threading;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendMessagesModels;
using Services.ViewModels.HomeModels;

namespace Services.Services.FacebookMessagesService
{
    public class FacebookStatusManager
    {
        private readonly IAccountManager _accountManager;

        public FacebookStatusManager()
        {
            _accountManager = new AccountManager();
        }

        public void MarkMessagesAsRead(UnreadFriendMessageList unreadMessages, AccountViewModel account)
        {
            foreach (var unreadMessage in unreadMessages.UnreadMessages)
            {
                var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
                {
                    UserAgentId = account.UserAgentId
                });

                new ChangeMessageStatusEngine().Execute(new ChangeMessageStatusModel
                {
                    AccountId = account.FacebookId,
                    FriendFacebookId = unreadMessage.FriendFacebookId,
                    Cookie = account.Cookie,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(
                        new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                        }),
                    UserAgent = userAgent.UserAgentString
                });

                Thread.Sleep(2000);
            }
        }

        public void MarkMessageAsRead(UnreadFriendMessageModel unreadMessage, AccountViewModel account)
        {
            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            new ChangeMessageStatusEngine().Execute(new ChangeMessageStatusModel
            {
                AccountId = account.FacebookId,
                FriendFacebookId = unreadMessage.FriendFacebookId,
                Cookie = account.Cookie,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                }),
                UserAgent = userAgent.UserAgentString
            });

            Thread.Sleep(2000);
        }
    }
}
