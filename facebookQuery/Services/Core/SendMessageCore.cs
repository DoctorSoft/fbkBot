using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeMessageRegimeCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.SendMessageEngine;
using Services.Core.Interfaces;
using Services.Core.Interfaces.ServiceTools;
using Services.Hubs;
using Services.ServiceTools;

namespace Services.Core
{
    public class SendMessageCore : ISendMessageCore
    {
        private readonly NotificationHub _notice;
        private readonly IFriendManager _friendManager;
        private readonly IAccountManager _accountManager;
        private readonly IFacebookMessageManager _facebookMessageManager;
        private readonly IMessageManager _messageManager;
        private readonly IStopWordsManager _stopWordsManager;

        public SendMessageCore()
        {
            _friendManager = new FriendManager();
            _accountManager = new AccountManager();
            _facebookMessageManager = new FacebookMessageManager();
            _messageManager = new MessageManager();
            _stopWordsManager = new StopWordsManager();
            _notice = new NotificationHub();
        }
        
        public void SendMessageToUnread(AccountModel account, FriendData friend)
        {
            if (friend.Deleted || friend.MessagesEnded)
            {
                _notice.Add(account.Id, string.Format("Ошибка! Друг {0}({1}) удален, либо переписка с ним закончилась.", friend.FriendName, friend.FacebookId));
                return;
            }

            var message = String.Empty;
            
            var messageData = friend.MessageRegime == MessageRegime.BotFirstMessage
            ? _messageManager.GetAllMessagesWhereBotWritesFirst(account.Id)
            : _messageManager.GetAllMessagesWhereUserWritesFirst(account.Id);

            _notice.Add(account.Id, string.Format("Загружаем сообщения для ответа"));

            if (messageData.Count == 0)
            {
                _notice.Add(account.Id, string.Format("У данного пользователя нет сообщений для ответа"));
                return;
            }

            var numberLastBotMessage = _messageManager.GetLasBotMessageOrderNumber(messageData, account.Id);

            //

            var lastFriendMessages = _facebookMessageManager.GetLastFriendMessageModel(account.Id, friend.Id);
            if (lastFriendMessages == null)
            {
                _notice.Add(account.Id, string.Format("Возникла ошибка при ответе. Сообщение друга не найдено в базе."));
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
                new ChangeMessageRegimeCommandHandler(new DataBaseContext()).Handle(new ChangeMessageRegimeCommand()
                {
                    AccountId = account.Id,
                    FriendId = friend.Id,
                    MessageRegime = MessageRegime.UserFirstMessage
                });

                friend.MessageRegime = MessageRegime.UserFirstMessage;
            }
            
            _notice.Add(account.Id, string.Format("Сверяем сообщение друга со стоп-словами"));

            var emergencyFactor = _stopWordsManager.CheckMessageOnEmergencyFaktor(lastFriendMessages);

            _notice.Add(account.Id, string.Format("Получаем сообщение для ответа с порядковым номером - {0}. (Стоп-фактор - {1})", orderNumber, emergencyFactor));

            var messageModel = _messageManager.GetRandomMessage(account.Id, orderNumber, emergencyFactor, friend.MessageRegime);

            if (messageModel != null)
            {
                _notice.Add(account.Id, string.Format("Подставляем значения в сообщение бота"));

                message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
                        {
                            TextPattern = messageModel.Message,
                            AccountId = account.Id,
                            FriendId = lastFriendMessages.FriendId,
                        });
            }

            if (message != String.Empty)
            {
                _notice.Add(account.Id, string.Format("Отправляем сообщение"));

                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.FacebookId,
                    Cookie = account.Cookie.CookieString,
                    FriendId = friend.FacebookId,
                    Message = message,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.SendMessage
                        })
                });

                new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
                {
                    AccountId = account.Id,
                    FriendId = friend.FacebookId,
                    OrderNumber = orderNumber,
                    Message = message,
                    MessageDateTime = DateTime.Now,
                });

                _notice.Add(account.Id, string.Format("Сообщение пользователю {0}({1}) отправлено",friend.FriendName,friend.FacebookId));
            }

            if (messageData == null || orderNumber < numberLastBotMessage)
            {
                return;
            }

            if (account.GroupSettingsId == null)
            {
                return;
            }

            _notice.Add(account.Id, string.Format("Переписка завершена. Блокируем пользователя {0}({1})", friend.FriendName, friend.FacebookId));

            _friendManager.AddFriendToBlackList((long)account.GroupSettingsId, friend.FacebookId);
        }

        public void SendMessageToUnanswered(AccountModel account, FriendData friend)
        {
            if (friend.MessagesEnded || friend.Deleted)
            {
              _notice.Add(account.Id, string.Format("Ошибка! Друг {0}({1}) удален, либо переписка с ним закончилась.", friend.FriendName, friend.FacebookId));
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

            _notice.Add(account.Id, string.Format("Получаем экстра-сообщение"));
              
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

            _notice.Add(account.Id, string.Format("Отправляем экстра сообщение - '{0}'", message));

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.FacebookId,
                Cookie = account.Cookie.CookieString,
                FriendId = friend.FacebookId,
                Message = message,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters =
                    new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.SendMessage
                    })
            });

            new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
            {
                AccountId = account.Id,
                FriendId = friend.FacebookId,
                OrderNumber = lastBotMessages.OrderNumber,
                Message = message,
                MessageDateTime = DateTime.Now,
            });

            _notice.Add(account.Id, string.Format("Отправка экстра сообщения завершена"));
        }

        public void SendMessageToNewFriend(AccountModel account, FriendData friend)
        {
            var message = String.Empty;

            _notice.Add(account.Id, string.Format("Отправляем сообщения новым друзьям"));

            var messageModel = _messageManager.GetRandomMessage(account.Id, 1, false, MessageRegime.BotFirstMessage);
            if (messageModel != null)
            {
                _notice.Add(account.Id, string.Format("Ошибка. Нет сообщений для отправки."));
                message = messageModel.Message;
            }

            if (!message.Equals(String.Empty))
            {
                _notice.Add(account.Id, string.Format("Отправляем сообщение {0}({1})", friend.FriendName, friend.FacebookId));

                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.FacebookId,
                    Cookie = account.Cookie.CookieString,
                    FriendId = friend.FacebookId,
                    Message = message,
                    Proxy = _accountManager.GetAccountProxy(account),
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.SendMessage
                        })
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

                _notice.Add(account.Id, string.Format("Сообщение отправлено"));
            }
        }
    }
}
