using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Engines.Engines.GetMessagesEngine.GetMessages;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Services.Core;
using Services.Hubs;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendMessagesModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.MessagesModels;

namespace Services.Services
{
    public class FacebookMessagesService
    {
        private readonly NotificationHub _notice;
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;

        public FacebookMessagesService()
        {
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
            _notice = new NotificationHub();
        }

        public void SendMessageToUnread(AccountViewModel accountViewModel)
        {
            _notice.Add(accountViewModel.Id, string.Format("Собираемся отвечать на непрочитанные сообщения"));

            var account = new AccountModel
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
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                },
                GroupSettingsId = accountViewModel.GroupSettingsId
            };

            var unreadMessagesList = GetUnreadMessages(account);

            _notice.Add(accountViewModel.Id, string.Format("Получено {0} непрочитанных сообщений", unreadMessagesList.UnreadMessages.Count));

            ChangeMessageStatus(unreadMessagesList, account);

            int i = 1;

            foreach (var unreadMessage in unreadMessagesList.UnreadMessages)
            {
                _notice.Add(account.Id, string.Format("Отвечаем на непрочитанные сообщения друзьям {0}/{1}", i, unreadMessagesList.UnreadMessages.Count));
                
                var friend = _friendManager.GetFriendByFacebookId(unreadMessage.FriendFacebookId);
                
                new SendMessageCore().SendMessageToUnread(account, friend);
                
                Thread.Sleep(3000);
                i++;
            }
        }

        public void SendMessageToUnanswered(AccountViewModel accountViewModel)
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
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                },
                GroupSettingsId = accountViewModel.GroupSettingsId
            };
            
            _notice.Add(accountViewModel.Id, string.Format("Собираемся посылать экстра-сообщения"));

            var unreadMessagesList = GetUnreadMessages(account);


            if (unreadMessagesList.UnreadMessages.Count != 0)
            {
                return;
            }

            if (account.GroupSettingsId == null)
            {
                return;
            }
            var settings = _accountSettingsManager.GetSettings((long)account.GroupSettingsId);

            if (settings == null)
            {
                return;
            }

            var delayTime = settings.UnansweredDelay;
            if (delayTime == null || delayTime < 0)
            {
                return;
            }

            var unansweredMessagesList =
                new GetUnansweredMessagesQueryHandler(new DataBaseContext()).Handle(new GetUnansweredMessagesQuery
                {
                    DelayTime = delayTime,
                    AccountId = account.Id
                });
            
            _notice.Add(accountViewModel.Id, string.Format("Получено {0} неотвеченных сообщений", unansweredMessagesList.Count));

            int i = 1;
            foreach (var unansweredMessage in unansweredMessagesList)
            {
                _notice.Add(account.Id, string.Format("Отправляем экстра-сообщения друзьям {0}/{1}", i, unansweredMessagesList.Count));
                var friend = _friendManager.GetFriendById(unansweredMessage.FriendId);

                new SendMessageCore().SendMessageToUnanswered(account, friend);

                Thread.Sleep(3000);

                i++;
            }
        }

        public void SendMessageToNewFriends(AccountViewModel accountViewModel)
        {
            var account = new AccountModel
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
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                },
                GroupSettingsId = accountViewModel.GroupSettingsId
            };

            _notice.Add(accountViewModel.Id, string.Format("Собираемся отправлять сообщения новым друзьям"));

            GetUnreadMessages(account);

            var newFriends = new GetNewFriendsForDialogueQueryHandler(new DataBaseContext()).Handle(new GetNewFriendsForDialogueQuery()
            {
                DelayTime = 10,
                AccountId = account.Id
            });

            _notice.Add(accountViewModel.Id, string.Format("Получено {0} новых друзей", newFriends.Count));

            int i = 1;
            foreach (var newFriend in newFriends)
            {
                _notice.Add(account.Id, string.Format("Отправляем сообщения новым друзьям {0}/{1}", i, newFriends.Count));
                new SendMessageCore().SendMessageToNewFriend(account, newFriend);

                Thread.Sleep(3000);

                i++;
            }
        }

        public UnreadFriendMessageList GetUnreadMessages(AccountModel account)
        {
            var unreadMessages = new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel()
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                })
            });

            if (unreadMessages.Count == 0)
            {
                return new UnreadFriendMessageList
                {
                    UnreadMessages = unreadMessages.Select(model => new UnreadFriendMessageModel
                    {
                        FriendFacebookId = model.FriendFacebookId,
                        UnreadMessage = model.UnreadMessage,
                        LastMessage = model.LastMessage,
                        CountAllMessages = model.CountAllMessages,
                        CountUnreadMessages = model.CountUnreadMessages,
                        LastReadMessageDateTime = model.LastReadMessageDateTime,
                        LastUnreadMessageDateTime = model.LastUnreadMessageDateTime,
                        FriendGender = model.Gender,
                        FriendName = model.Name,
                        FriendHref = model.Href
                    }).ToList()
                };
            }

            new SaveUnreadMessagesCommandHandler(new DataBaseContext()).Handle(new SaveUnreadMessagesCommand()
            {
                AccountId = account.Id,
                UnreadMessages = unreadMessages.Select(model => new FacebookMessageDbModel()
                {
                    AccountId = model.AccountId,
                    FriendFacebookId = model.FriendFacebookId,
                    Gender = model.Gender,
                    CountAllMessages = model.CountAllMessages,
                    CountUnreadMessages = model.CountUnreadMessages,
                    Href = model.Href,
                    LastMessage = model.Href,
                    LastReadMessageDateTime = model.LastReadMessageDateTime,
                    LastUnreadMessageDateTime = model.LastUnreadMessageDateTime,
                    Name = model.Name,
                    UnreadMessage = model.UnreadMessage
                }).ToList()
            });

            var ureadMessages = unreadMessages.Select(model => new UnreadFriendMessageModel
            {
                FriendFacebookId = model.FriendFacebookId,
                UnreadMessage = model.UnreadMessage,
                LastMessage = model.LastMessage,
                CountAllMessages = model.CountAllMessages,
                CountUnreadMessages = model.CountUnreadMessages,
                LastReadMessageDateTime = model.LastReadMessageDateTime,
                LastUnreadMessageDateTime = model.LastUnreadMessageDateTime,
                FriendGender = model.Gender,
                FriendName = model.Name,
                FriendHref = model.Href
            }).ToList();

            return new UnreadFriendMessageList()
            {
                UnreadMessages = ureadMessages
            };
        }

        public void ChangeMessageStatus(UnreadFriendMessageList unreadMessages, AccountModel account)
        {
            foreach (var unreadMessage in unreadMessages.UnreadMessages)
            {
                new ChangeMessageStatusEngine().Execute(new ChangeMessageStatusModel
                {
                    AccountId = account.FacebookId,
                    FriendFacebookId = unreadMessage.FriendFacebookId,
                    Cookie = account.Cookie.CookieString,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                    }),
                });

                Thread.Sleep(2000);
            }
        }

        public UnreadMessagesListViewModel GetUnreadMessagesFromAccountPage(long accountId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountId
            });

            var getUnreadMessagesUrlParameters =
                new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                });

            var unreadMessagesList =  new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie.CookieString,
                UrlParameters = getUnreadMessagesUrlParameters,
                Proxy = _accountManager.GetAccountProxy(account)
            });

            return new UnreadMessagesListViewModel()
            {
                UnreadMessages = unreadMessagesList.Select(model=> new UnreadMessageModel
                {
                    LastMessage = model.LastMessage,
                    UnreadMessage = model.UnreadMessage,
                    CountAllMessages = model.CountAllMessages,
                    CountUnreadMessages = model.CountUnreadMessages,
                    FacebookFriendId = model.FriendFacebookId
                }).ToList()
            };
        }
        
        public List<GetMessagesResponseModel> GetAllMessages(long accountId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountId
            });
            return new GetMessagesEngine().Execute(new GetMessagesModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account)
            });
        }

        public void GetСorrespondenceByFriendId(long accountId, long friendId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountId
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetCorrespondence
            });

            var correspondence = new GetСorrespondenceByFriendIdEngine().Execute(new GetСorrespondenceByFriendIdModel()
            {
                Cookie = account.Cookie.CookieString,
                AccountId = accountId,
                FriendId = friendId,
                UrlParameters = urlParameters,
                Proxy = _accountManager.GetAccountProxy(account)
            });
        }
    }
}
