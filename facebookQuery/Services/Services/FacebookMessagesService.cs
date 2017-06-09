using System;
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
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Engines.Engines.GetMessagesEngine.GetCorrespondenceRequests;
using Engines.Engines.GetMessagesEngine.GetMessages;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Engines.Engines.Models;
using Services.Core;
using Services.Interfaces.Notices;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendMessagesModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.MessagesModels;

namespace Services.Services
{
    public class FacebookMessagesService
    {
        private readonly INotices _notice;
        private readonly NoticeService _noticeService;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;

        public FacebookMessagesService(INotices noticeProxy)
        {
            _noticeService  = new NoticeService();
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
            _seleniumManager = new SeleniumManager();
            _notice = noticeProxy;
        }

        public void SendMessageToUnread(AccountViewModel accountViewModel)
        {
            const string functionName = "Ответ на непрочитанные сообщения";

            _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Начинаем отвечать"));

            var account = _accountManager.GetAccountById(accountViewModel.Id);

            try
            {
                var unreadMessagesList = GetUnreadMessages(account);

                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Получено {0} непрочитанных сообщений", unreadMessagesList.UnreadMessages.Count));

                ChangeMessageStatus(unreadMessagesList, account);

                var i = 1;

                foreach (var unreadMessage in unreadMessagesList.UnreadMessages)
                {
                    _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Отвечаем на непрочитанные сообщения друзьям {0}/{1}", i, unreadMessagesList.UnreadMessages.Count));

                    var friend = _friendManager.GetFriendByFacebookId(unreadMessage.FriendFacebookId);

                    new SendMessageCore(_notice).SendMessageToUnread(account, friend);

                    Thread.Sleep(3000);
                    i++;
                }
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Успешно завершено"));
            }
            catch (Exception ex)
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Возникла ошибка {0}", ex.Message));
            }
        }

        public void SendMessageToUnanswered(AccountViewModel accountViewModel)
        {
            const string functionName = "Ответ на неотвеченные сообщения";

            _notice.AddNotice(functionName, accountViewModel.Id, _noticeService.ConvertNoticeText(functionName, string.Format("Начинаем отвечать")));

            var account = _accountManager.GetAccountById(accountViewModel.Id);

            try
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Собираемся посылать экстра-сообщения"));

                var unreadMessagesList = GetUnreadMessages(account);


                if (unreadMessagesList.UnreadMessages.Count != 0)
                {
                    return;
                }

                if (account.GroupSettingsId == null)
                {
                    return;
                }
                var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);

                if (settings == null)
                {
                    return;
                }

                var delayTime = settings.UnansweredDelay;
                if (delayTime < 0)
                {
                    return;
                }

                var unansweredMessagesList =
                    new GetUnansweredMessagesQueryHandler(new DataBaseContext()).Handle(new GetUnansweredMessagesQuery
                    {
                        DelayTime = delayTime,
                        AccountId = account.Id
                    });

                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Получено {0} неотвеченных сообщений", unansweredMessagesList.Count));

                int i = 1;
                foreach (var unansweredMessage in unansweredMessagesList)
                {
                    _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Отправляем экстра-сообщения друзьям {0}/{1}", i, unansweredMessagesList.Count));
                    var friend = _friendManager.GetFriendById(unansweredMessage.FriendId);

                    new SendMessageCore(_notice).SendMessageToUnanswered(account, friend);

                    Thread.Sleep(3000);

                    i++;
                }
            }
            catch (Exception ex)
            {
                _notice.AddNotice(functionName, accountViewModel.Id, _noticeService.ConvertNoticeText(functionName, string.Format("Возникла ошибка {0}", ex.Message)));
            }
        }

        public void SendMessageToNewFriends(AccountViewModel accountViewModel)
        {
            const string functionName = "Посылаем сообщения новым друзьям";

            _notice.AddNotice(functionName, accountViewModel.Id, _noticeService.ConvertNoticeText(functionName, string.Format("Начинаем посылать")));

            var account = _accountManager.GetAccountById(accountViewModel.Id);

            try
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Собираемся отправлять сообщения новым друзьям"));

                GetUnreadMessages(account);

                var newFriends =
                    new GetNewFriendsForDialogueQueryHandler(new DataBaseContext()).Handle(new GetNewFriendsForDialogueQuery
                    {
                        DelayTime = 10,
                        CountFriend = 10,
                        AccountId = account.Id
                    });

                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Получено {0} новых друзей", newFriends.Count));

                int i = 1;
                foreach (var newFriend in newFriends)
                {
                    _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Отправляем сообщения новым друзьям {0}/{1}", i, newFriends.Count));
                    new SendMessageCore(_notice).SendMessageToNewFriend(account, newFriend);

                    Thread.Sleep(3000);

                    i++;
                }
            }
            catch (Exception ex)
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Возникла ошибка {0}", ex.Message));
            }
        }

        public UnreadFriendMessageList GetUnreadMessages(AccountViewModel account)
        {
            var unreadMessages = new List<FacebookMessageModel>();
            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            var newCorrespondenceRequests = new GetCorrespondenceRequestsEndine().Execute(new GetCorrespondenceRequestsModel
                {
                    AccountFacebookId = account.FacebookId,
                    Cookie = account.Cookie,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Driver = _seleniumManager.RegisterNewDriver(new AccountViewModel
                    {
                        ProxyLogin = account.ProxyLogin,
                        ProxyPassword = account.ProxyPassword,
                        Proxy = account.Proxy,
                        UserAgentId = account.UserAgentId
                    })
                });

            unreadMessages.AddRange(newCorrespondenceRequests);

            var newUnreadMessages = new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                }),
                UserAgent = userAgent.UserAgentString
            });

            foreach (var facebookMessageModel in newUnreadMessages)
            {
                if (
                    newCorrespondenceRequests.Any(
                        model => model.FriendFacebookId == facebookMessageModel.FriendFacebookId))
                {
                    continue;
                }

                unreadMessages.Add(facebookMessageModel);
            }

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
                        FriendHref = model.Href,
                        FriendType = model.FriendType
                    }).ToList()
                };
            }

            new SaveUnreadMessagesCommandHandler(new DataBaseContext()).Handle(new SaveUnreadMessagesCommand()
            {
                AccountId = account.Id,
                UnreadMessages = unreadMessages.Select(model => new FacebookMessageDbModel
                {
                    AccountId = model.AccountId,
                    FriendFacebookId = model.FriendFacebookId,
                    Gender = model.Gender,
                    CountAllMessages = model.CountAllMessages,
                    CountUnreadMessages = model.CountUnreadMessages,
                    Href = model.Href,
                    LastMessage = model.LastMessage,
                    LastReadMessageDateTime = model.LastReadMessageDateTime,
                    LastUnreadMessageDateTime = model.LastUnreadMessageDateTime,
                    Name = model.Name,
                    UnreadMessage = model.UnreadMessage,
                    FriendType = model.FriendType
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

            return new UnreadFriendMessageList
            {
                UnreadMessages = ureadMessages
            };
        }

        public void ChangeMessageStatus(UnreadFriendMessageList unreadMessages, AccountViewModel account)
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
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                    }),
                    UserAgent = userAgent.UserAgentString
                });

                Thread.Sleep(2000);
            }
        }

        public UnreadMessagesListViewModel GetUnreadMessagesFromAccountPage(long accountId)
        {
            var account = _accountManager.GetAccountById(accountId);

            var getUnreadMessagesUrlParameters =
                new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                });

            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            var unreadMessagesList =  new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie,
                UrlParameters = getUnreadMessagesUrlParameters,
                Proxy = _accountManager.GetAccountProxy(account),
                UserAgent = userAgent.UserAgentString
            });

            return new UnreadMessagesListViewModel
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

        public void GetСorrespondenceByFriendId(long accountFacebookId, long friendId)
        {
            var account = _accountManager.GetAccountByFacebookId(accountFacebookId);

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetCorrespondence
            });

            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            var correspondence = new GetСorrespondenceByFriendIdEngine().Execute(new GetСorrespondenceByFriendIdModel()
            {
                Cookie = account.Cookie,
                AccountFacebookId = accountFacebookId,
                FriendId = friendId,
                UrlParameters = urlParameters,
                Proxy = _accountManager.GetAccountProxy(account),
                UserAgent = userAgent.UserAgentString
            });
        }
    }
}
