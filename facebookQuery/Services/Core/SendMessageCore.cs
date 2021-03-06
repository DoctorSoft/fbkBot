﻿using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeMessageRegimeCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToEndDialogCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using Engines.Engines.SendMessageEngine;
using Services.Core.Interfaces;
using Services.Interfaces.Notices;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Services.Core
{
    public class SendMessageCore : ISendMessageCore
    {
        private readonly INotices _notice;
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IFacebookMessageManager _facebookMessageManager;
        private readonly IMessageManager _messageManager;
        private readonly IStopWordsManager _stopWordsManager;
        private readonly IFriendsBlackListManager _friendsBlackListManager;

        public SendMessageCore(INotices noticeProxy)
        {
            _friendsBlackListManager = new FriendsBlackListManager();
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _facebookMessageManager = new FacebookMessageManager();
            _messageManager = new MessageManager();
            _stopWordsManager = new StopWordsManager();
            _notice = noticeProxy;
        }
        
        public void SendMessageToUnread(AccountViewModel account, FriendData friend)
        {
            const string functionName = "Ответ на непрочитанные сообщения";

            if (friend.Deleted || friend.DialogIsCompleted)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) удален, либо переписка с ним закончилась.", friend.FriendName, friend.FacebookId));
                return;
            }

            if (account.GroupSettingsId == null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Не указана группа настроек."));
               return;
            }

            var friendIsBlocked = _friendsBlackListManager.CheckForFriendBlacklist(friend.FacebookId, (long)account.GroupSettingsId);
            if (friendIsBlocked)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) находится в черном списке.", friend.FriendName, friend.FacebookId));
                return;
            }

            var message = String.Empty;
            
            var messageData = friend.MessageRegime == MessageRegime.BotFirstMessage
            ? _messageManager.GetAllMessagesWhereBotWritesFirst(account.Id)
            : _messageManager.GetAllMessagesWhereUserWritesFirst(account.Id);

            _notice.AddNotice(functionName, account.Id, string.Format("Загружаем сообщения для ответа"));

            if (messageData.Count == 0)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("У данного пользователя нет сообщений для ответа"));
                return;
            }

            var numberLastBotMessage = _messageManager.GetLasBotMessageOrderNumber(messageData, account.Id);

            //

            var lastFriendMessages = _facebookMessageManager.GetLastFriendMessageModel(account.Id, friend.Id);
            if (lastFriendMessages == null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Возникла ошибка при ответе. Сообщение друга не найдено в базе."));
                return;
            }

            var lastBotMessage = _facebookMessageManager.GetLastBotMessageModel(account.Id, friend.Id);

            int orderNumber;
            if (lastBotMessage != null && lastFriendMessages.OrderNumber == lastBotMessage.OrderNumber && lastFriendMessages.MessageDateTime > lastBotMessage.MessageDateTime)
            {
                orderNumber = lastFriendMessages.OrderNumber + 1;
            }
            else
            {
                orderNumber = lastFriendMessages.OrderNumber;
            }
            
            if (orderNumber == 1 && friend.MessageRegime == null)
            {
                new ChangeMessageRegimeCommandHandler(new DataBaseContext()).Handle(new ChangeMessageRegimeCommand
                {
                    AccountId = account.Id,
                    FriendId = friend.Id,
                    MessageRegime = MessageRegime.UserFirstMessage
                });

                friend.MessageRegime = MessageRegime.UserFirstMessage;
            }
            
            _notice.AddNotice(functionName, account.Id, string.Format("Сверяем сообщение друга со стоп-словами"));

            var emergencyFactor = _stopWordsManager.CheckMessageOnEmergencyFaktor(lastFriendMessages);

            _notice.AddNotice(functionName, account.Id, string.Format("Получаем сообщение для ответа с порядковым номером - {0}. (Стоп-фактор - {1})", orderNumber, emergencyFactor));

            var messageModel = _messageManager.GetRandomMessage(account.Id, orderNumber, emergencyFactor, friend.MessageRegime);

            if (messageModel != null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Подставляем значения в сообщение бота"));

                message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
                        {
                            TextPattern = messageModel.Message,
                            AccountId = account.Id,
                            FriendId = lastFriendMessages.FriendId,
                        });
            }
            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            if (message != String.Empty)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Отправляем сообщение"));

                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.FacebookId,
                    Cookie = account.Cookie,
                    FriendId = friend.FacebookId,
                    Message = message,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.SendMessage
                        }),
                    UserAgent = userAgent.UserAgentString
                });

                new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
                {
                    AccountId = account.Id,
                    FriendId = friend.FacebookId,
                    OrderNumber = orderNumber,
                    Message = message,
                    MessageDateTime = DateTime.Now,
                });

                _notice.AddNotice(functionName, account.Id, string.Format("Сообщение пользователю {0}({1}) отправлено",friend.FriendName,friend.FacebookId));
            }

            if (messageData == null || orderNumber < numberLastBotMessage)
            {
                return;
            }

            if (account.GroupSettingsId == null)
            {
                return;
            }

            _notice.AddNotice(functionName, account.Id, string.Format("Переписка завершена. Блокируем пользователя {0}({1})", friend.FriendName, friend.FacebookId));

            new MarkAddToEndDialogCommandHandler(new DataBaseContext()).Handle(new MarkAddToEndDialogCommand
            {
                AccountId = account.Id,
                FriendId = friend.FacebookId
            });

            _friendManager.AddFriendToBlackList((long)account.GroupSettingsId, friend.FacebookId);
        }

        public void SendMessageToUnanswered(AccountViewModel account, FriendData friend)
        {
            const string functionName = "Ответ на неотвеченные сообщения";

            if (friend.DialogIsCompleted || friend.Deleted)
            {
              _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) удален, либо переписка с ним закончилась.", friend.FriendName, friend.FacebookId));
              return;
            }

            if (account.GroupSettingsId == null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Не указана группа настроек."));
                return;
            }

            var friendIsBlocked = _friendsBlackListManager.CheckForFriendBlacklist(friend.FacebookId, (long)account.GroupSettingsId);
            if (friendIsBlocked)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) находится в черном списке.", friend.FriendName, friend.FacebookId));
                return;
            }

            var allMessages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = account.Id,
                FriendId = friend.Id
            });

            var lastBotMessages =
            allMessages.Where(data => data.MessageDirection == MessageDirection.ToFriend)
                .OrderByDescending(data => data.OrderNumber)
                .FirstOrDefault();
            
            if (lastBotMessages == null)
            {
                return;
            }

            _notice.AddNotice(functionName, account.Id, string.Format("Получаем экстра-сообщение"));
              
            var messageModel = _messageManager.GetRandomExtraMessage();

            if (messageModel == null)
            {
                return;
            }

            var message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
            {
                TextPattern = messageModel.Message,
                AccountId = account.Id,
                FriendId = friend.FacebookId
            });

            _notice.AddNotice(functionName, account.Id, string.Format("Отправляем экстра сообщение - '{0}'", message));
            
            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie,
                FriendId = friend.FacebookId,
                Message = message,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters =
                    new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.SendMessage
                    }),
                UserAgent = userAgent.UserAgentString
            });

            new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
            {
                AccountId = account.Id,
                FriendId = friend.FacebookId,
                OrderNumber = lastBotMessages.OrderNumber,
                Message = message,
                MessageDateTime = DateTime.Now,
            });

            _notice.AddNotice(functionName, account.Id, string.Format("Отправка экстра сообщения завершена"));
        }

        public void SendMessageToNewFriend(AccountViewModel account, FriendData friend)
        {
            const string functionName = "Отправка сообщений новым друзьям";

            if (friend.DialogIsCompleted || friend.Deleted)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) удален, либо переписка с ним закончилась.", friend.FriendName, friend.FacebookId));
                return;
            }

            if (account.GroupSettingsId == null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Не указана группа настроек."));
                return;
            }

            var friendIsBlocked = _friendsBlackListManager.CheckForFriendBlacklist(friend.FacebookId, (long)account.GroupSettingsId);
            if (friendIsBlocked)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка! Друг {0}({1}) находится в черном списке.", friend.FriendName, friend.FacebookId));
                return;
            }

            var message = String.Empty;

            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            _notice.AddNotice(functionName, account.Id, string.Format("Отправляем сообщения новым друзьям"));

            var messageModel = _messageManager.GetRandomMessage(account.Id, 1, false, MessageRegime.BotFirstMessage);
            if (messageModel == null)
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Ошибка. Нет сообщений для отправки."));

                return;
            }

            message = messageModel.Message;

            if (!message.Equals(String.Empty))
            {
                _notice.AddNotice(functionName, account.Id, string.Format("Отправляем сообщение {0}({1})", friend.FriendName, friend.FacebookId));

                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.FacebookId,
                    Cookie = account.Cookie,
                    FriendId = friend.FacebookId,
                    Message = message,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.SendMessage
                        }),
                    UserAgent = userAgent.UserAgentString
                });

                if (friend.MessageRegime == null)
                {
                    new ChangeMessageRegimeCommandHandler(new DataBaseContext()).Handle(new ChangeMessageRegimeCommand()
                    {
                        AccountId = account.Id,
                        FriendId = friend.Id,
                        MessageRegime = MessageRegime.BotFirstMessage
                    });
                }

                new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
                {
                    AccountId = account.Id,
                    FriendId = friend.FacebookId,
                    OrderNumber = 1,
                    Message = message,
                    MessageDateTime = DateTime.Now
                });

                _notice.AddNotice(functionName, account.Id, string.Format("Сообщение отправлено"));
            }
        }
    }
}
