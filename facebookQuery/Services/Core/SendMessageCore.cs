using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeMessageRegimeCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.SendMessageEngine;
using Services.Core.Interfaces;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;

namespace Services.Core
{
    public class SendMessageCore : ISendMessageCore
    {
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
        }
        
        public void SendMessageToUnread(AccountModel account, FriendData friend)
        {
            if (friend.Deleted || friend.MessagesEnded)
            {
                return;
            }

            var message = String.Empty;
            
            //
            var messageData = friend.MessageRegime == MessageRegime.BotFirstMessage
            ? _messageManager.GetAllMessagesWhereBotWritesFirst(account.Id)
            : _messageManager.GetAllMessagesWhereUserWritesFirst(account.Id);

            if (messageData.Count == 0)
            {
                return;
            }
            var numberLastBotMessage = _messageManager.GetLasBotMessageOrderNumber(messageData, account.Id);

            //

            var lastFriendMessages = _facebookMessageManager.GetLastFriendMessageModel(account.Id, friend.Id);
            if (lastFriendMessages == null)
            {
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

            var emergencyFactor = _stopWordsManager.CheckMessageOnEmergencyFaktor(lastFriendMessages);

            var messageModel = _messageManager.GetRandomMessage(account.Id, orderNumber, emergencyFactor, friend.MessageRegime);

            if (messageModel != null)
            {
                message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
                        {
                            TextPattern = messageModel.Message,
                            AccountId = account.Id,
                            FriendId = lastFriendMessages.FriendId,
                        });
            }

            if (message != String.Empty)
            {
                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.UserId,
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
            }
            if (messageData != null && orderNumber >= numberLastBotMessage)
            {
                new MarkBlockedFriendCommandHandler(new DataBaseContext()).Handle(new MarkBlockedFriendCommand()
                {
                    AccountId = account.Id,
                    FriendId = lastFriendMessages.FriendId
                });
            }
        }

        public void SendMessageToUnanswered(AccountModel account, FriendData friend)
        {
            if (friend.MessagesEnded || friend.Deleted)
            {
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

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.UserId,
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
        }

        public void SendMessageToNewFriend(AccountModel account, FriendData friend)
        {
            var message = String.Empty;

            var messageModel = _messageManager.GetRandomMessage(account.Id, 1, false, MessageRegime.BotFirstMessage);
            if (messageModel != null)
            {
                message = messageModel.Message;
            }

            if (!message.Equals(String.Empty))
            {
                new SendMessageEngine().Execute(new SendMessageModel
                {
                    AccountId = account.UserId,
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
                        AccountId = account.UserId,
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
            }
        }
    }
}
