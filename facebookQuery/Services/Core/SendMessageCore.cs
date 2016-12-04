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
            
            var messageData = _messageManager.GetAllMessagesWhereUserWritesFirst(account.Id);

            if (messageData.Count == 0)
            {
                return;
            }
            var numberLastBotMessage = _messageManager.GetLasBotMessageOrderNumber(messageData, account.Id);

            var lastFriendMessages = _facebookMessageManager.GetLastFriendMessageModel(account.Id, friend.Id);
            if (lastFriendMessages == null)
            {
                return;
            }

            var orderNumber = lastFriendMessages.OrderNumber;
            if (orderNumber == 1 && friend.MessageRegime == null)
            {
                new ChangeMessageRegimeCommandHandler(new DataBaseContext()).Handle(new ChangeMessageRegimeCommand()
                {
                    AccountId = account.Id,
                    FriendId = friend.Id,
                    MessageRegime = MessageRegime.UserFirstMessage
                });
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

        public void SendMessageToUnanswered(long senderId, long friendId)
        {
            var friend = _friendManager.GetFriendById(friendId);

            var account = _accountManager.GetAccountByFacebookId(senderId);
            
            if (friend == null)
            {
                new SaveNewFriendCommandHandler(new DataBaseContext()).Handle(new SaveNewFriendCommand()
                {
                    AccountId = account.Id,
                    FriendData = new FriendData()
                    {
                        
                    }
                });
            }

            if (friend.MessagesEnded || friend.Deleted)
            {
                return;
            }

            var allMessages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = account.Id,
                FriendId = friend.Id
            });

            var lastFriendMessages =
            allMessages.Where(data => data.MessageDirection == MessageDirection.FromFriend)
                .OrderByDescending(data => data.OrderNumber)
                .FirstOrDefault();

            var lastBotMessages =
            allMessages.Where(data => data.MessageDirection == MessageDirection.ToFriend)
                .OrderByDescending(data => data.OrderNumber)
                .FirstOrDefault();

            var messageData = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
            {
                AccountId = account.Id
            }).Where(model => model.MessageRegime == MessageRegime.UserFirstMessage).ToList();

            var numberLastResponseMessage = 0;
            var lastResponseMessageModel = messageData.OrderByDescending(data => data.OrderNumber).FirstOrDefault();
            if (lastResponseMessageModel != null)
            {
                numberLastResponseMessage = lastResponseMessageModel.OrderNumber;
            }

            if (lastFriendMessages == null || lastBotMessages == null)
            {
                return;
            }

            var orderNumber = lastBotMessages.OrderNumber + 1;

            var emergencyFactor = _stopWordsManager.CheckMessageOnEmergencyFaktor(lastFriendMessages);

            var messageModel = _messageManager.GetRandomMessage(account.Id, orderNumber, emergencyFactor, friend.MessageRegime);

            if (messageModel == null)
            {
                return;
            }

            var message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery
            {
                TextPattern = messageModel.Message,
                AccountId = account.Id,
                FriendId = lastFriendMessages.FriendId
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
                OrderNumber = orderNumber,
                Message = message,
                MessageDateTime = DateTime.Now,
            });

            if (messageData != null && orderNumber >= numberLastResponseMessage)
            {
                new MarkBlockedFriendCommandHandler(new DataBaseContext()).Handle(new MarkBlockedFriendCommand()
                {
                    AccountId = account.Id,
                    FriendId = lastFriendMessages.FriendId
                });
            }
        }

        public void SendMessageToNewFriend(long senderId, long friendId)
        {
            var message = String.Empty;

            var account = _accountManager.GetAccountById(senderId);
            var friend = _friendManager.GetFriendByFacebookId(friendId);

            var messageModel = _messageManager.GetRandomMessage(account.Id, 1, false, MessageRegime.BotFirstMessage);
            if (messageModel != null)
            {
                message = messageModel.Message;
            }

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                FriendId = friendId,
                Message = message,
                Proxy = _accountManager.GetAccountProxy(account),
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
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
                FriendId = friendId,
                OrderNumber = 1,
                Message = message,
                MessageDateTime = DateTime.Now
            });
        }
    }
}
