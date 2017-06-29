using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using Engines.Engines.GetMessagesEngine.GetCorrespondenceRequests;
using Engines.Engines.GetMessagesEngine.GetMessangerMessages;
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

namespace Services.Services.FacebookMessagesService
{
    public class FacebookMessagesService
    {
        private readonly INotices _notice;
        private readonly NoticeService _noticeService;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IMessageManager _messageManager;
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly FacebookStatusManager _facebookStatusManager;

        public FacebookMessagesService(INotices noticeProxy)
        {
            _noticeService  = new NoticeService();
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
            _seleniumManager = new SeleniumManager();
            _notice = noticeProxy;
            _facebookStatusManager = new FacebookStatusManager();
            _messageManager = new MessageManager();
        }

        public void SendMessageToUnread(AccountViewModel accountViewModel)
        {
            const string functionName = "Ответ на непрочитанные сообщения";

            _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Начинаем отвечать"));

            try
            {
                var account = _accountManager.GetAccountById(accountViewModel.Id);
                if (account.GroupSettingsId == null)
                {
                    _notice.AddNotice(functionName, account.Id, "Ошибка! Не указана группа настроек.");
                    return;
                }

                var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);

                var unreadMessagesList = GetUnreadMessages(account);
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Получено {0} непрочитанных сообщений", unreadMessagesList.UnreadMessages.Count));
                SendMessages(unreadMessagesList.UnreadMessages, account, functionName);

                if (settings.GetMessagesFromThoseConnectedToMessenger)
                {
                    var newMessangerMessagesList = GetMessangerMessages(account);
                    _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Получено {0} сообщений о добавлении пользователя в мессенджер", newMessangerMessagesList.UnreadMessages.Count));
                    SendMessages(newMessangerMessagesList.UnreadMessages, account, functionName);
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

            _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Начинаем отвечать"));

            var account = _accountManager.GetAccountById(accountViewModel.Id);

            try
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Собираемся посылать экстра-сообщения"));

                var unreadMessagesList = GetUnreadMessages(account);
                List<UnreadFriendMessageModel> unreadMessages = null;


                if (unreadMessagesList != null)
                {
                    unreadMessages = unreadMessagesList.UnreadMessages;
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
                if (delayTime <= 0)
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
                _notice.AddNotice(functionName, account.Id, "Загружаем список  extra-сообщений");

                var messageModel = _messageManager.GetRandomExtraMessage();

                if (messageModel == null)
                {
                    _notice.AddNotice(functionName, account.Id, "Нет ни одного extra-сообщения");
                    return;
                }

                int i = 1;
                foreach (var unansweredMessage in unansweredMessagesList)
                {
                    if (unreadMessages != null &&
                        unreadMessages.Any(model => model.FriendFacebookId == unansweredMessage.FriendFacebookId))
                    {
                        _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Друг с id = {0} написал сообщение. Экстра сообщение не посылаем ему.", unansweredMessage.FriendFacebookId));
                        continue;
                    }

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

            _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Начинаем посылать"));

            var account = _accountManager.GetAccountById(accountViewModel.Id);
            if (account.GroupSettingsId == null)
            {
                _notice.AddNotice(functionName, account.Id, "Ошибка! Не указана группа настроек.");
                return;
            }
            try
            {
                _notice.AddNotice(functionName, accountViewModel.Id, string.Format("Собираемся отправлять сообщения новым друзьям"));

                GetUnreadMessages(account);

                var newFriends =
                    new GetNewFriendsForDialogueQueryHandler(new DataBaseContext()).Handle(new GetNewFriendsForDialogueQuery
                    {
                        DelayTime = 10,
                        CountFriend = 10,
                        AccountId = account.Id,
                        GroupSettingsId = (long)account.GroupSettingsId
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
            const string functionName = "Получение непрочитанных сообщений";
            var unreadMessages = new List<FacebookMessageModel>();
            List<UnreadFriendMessageModel> ureadMessages = null;

            try
            {
                var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
                {
                    UserAgentId = account.UserAgentId
                });

                var newCorrespondenceRequests = new GetCorrespondenceRequestsEndine().Execute(
                    new GetCorrespondenceRequestsModel
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
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(
                        new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                        }),
                    UserAgent = userAgent.UserAgentString,
                    NumbersOfDialogues = 10
                });

                foreach (var facebookMessageModel in newUnreadMessages)
                {
                    if (newCorrespondenceRequests.Any(model => model.FriendFacebookId == facebookMessageModel.FriendFacebookId))
                    {
                        continue;
                    }

                    if (facebookMessageModel.LastMessage.Equals(string.Empty))
                    {
                        //todo: MARK UNREAD
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

                ureadMessages = unreadMessages.Select(model => new UnreadFriendMessageModel
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
            }
            catch (Exception ex)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Возникла ошибка {0}", ex.Message));
            }
            return new UnreadFriendMessageList
            {
                UnreadMessages = ureadMessages
            };
        }

        private UnreadFriendMessageList GetMessangerMessages(AccountViewModel account)
        {
            const string noticeName = "Теперь вы общаетесь и в Messenger";
            var functionName = $"Получение сообщений '{noticeName}'";

            try
            {
                var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
                {
                    UserAgentId = account.UserAgentId
                });
                
                var messages = new GetMessangerMessagesEngine().Execute(new GetMessangerMessagesModel
                {
                    AccountId = account.FacebookId,
                    Cookie = account.Cookie,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(
                        new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.GetMessangerMessages
                        }),
                    UserAgent = userAgent.UserAgentString,
                    NumbersOfDialogues = 10
                });
                
                var result = new UnreadFriendMessageList
                {
                    UnreadMessages = messages.Select(model => new UnreadFriendMessageModel
                    {
                        FriendFacebookId = model.FriendFacebookId,
                        UnreadMessage = model.UnreadMessage,
                        LastMessage = noticeName,
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
                

                new SaveUnreadMessagesCommandHandler(new DataBaseContext()).Handle(new SaveUnreadMessagesCommand
                {
                    AccountId = account.Id,
                    UnreadMessages = messages.Select(model => new FacebookMessageDbModel
                    {
                        AccountId = model.AccountId,
                        FriendFacebookId = model.FriendFacebookId,
                        Gender = model.Gender,
                        CountAllMessages = model.CountAllMessages,
                        CountUnreadMessages = model.CountUnreadMessages,
                        Href = model.Href,
                        LastMessage = noticeName,
                        LastReadMessageDateTime = model.LastReadMessageDateTime,
                        LastUnreadMessageDateTime = model.LastUnreadMessageDateTime,
                        Name = model.Name,
                        UnreadMessage = model.UnreadMessage,
                        FriendType = model.FriendType
                    }).ToList()
                });

                return result;
            }
            catch (Exception ex)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Возникла ошибка {0}", ex.Message));
            }

            return null;
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

        private void SendMessages(List<UnreadFriendMessageModel> unreadMessages, AccountViewModel account, string functionName)
        {
            var i = 1;

            foreach (var unreadMessage in unreadMessages)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Отвечаем на непрочитанные сообщения друзьям {0}/{1}", i, unreadMessages.Count));

                var friend = _friendManager.GetFriendByFacebookId(unreadMessage.FriendFacebookId);

                _facebookStatusManager.MarkMessageAsRead(unreadMessage, account);
                new SendMessageCore(_notice).SendMessageToUnread(account, friend);

                Thread.Sleep(3000);
                i++;
            }
        }
    }
}
