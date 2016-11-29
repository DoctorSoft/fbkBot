using System;
using System.Collections.Generic;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.Migrations;
using DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.SendMessageEngine;

namespace Services.Core
{
    public class SendMessageCore
    {
        public void SendMessageToUnread(long senderId, long friendId)
        {
            var message = String.Empty;

            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = senderId
            });

            var friend = new GetFriendByIdFacebookQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdFacebookQuery
                {
                    FacebookId = friendId
                });

            var lastFriendMessages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = account.Id,
                FriendId = friend.Id
            }).Where(data => data.MessageDirection == MessageDirection.FromFriend)
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

            if (lastFriendMessages != null)
            {
                var orderNumber = lastFriendMessages.OrderNumber;

                var messageModel = GetRandomMessage(messageData, orderNumber);
                if (messageModel != null)
                {
                    message = messageModel.Message;
                }

                if (message != String.Empty)
                {
                    new SendMessageEngine().Execute(new SendMessageModel
                    {
                        AccountId = account.UserId,
                        Cookie = account.Cookie.CookieString,
                        FriendId = friendId,
                        Message = new CalculateMessageTextQueryHandler(new DataBaseContext()).Handle(new CalculateMessageTextQuery()
                        {
                            TextPattern = message,
                            AccountId = account.Id,
                            FriendId = lastFriendMessages.FriendId,

                        }),
                        UrlParameters =
                            new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                            {
                                NameUrlParameter = NamesUrlParameter.SendMessage
                            })
                    });
                
                    new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
                    {
                        AccountId = account.Id,
                        FriendId = friendId,
                        OrderNumber = orderNumber,
                        Message = message,
                        MessageDateTime = DateTime.Now
                    });
                }
                if (messageData != null && orderNumber >= numberLastResponseMessage)
                {
                    new MarkBlockedFriendCommandHandler(new DataBaseContext()).Handle(new MarkBlockedFriendCommand()
                    {
                        AccountId = account.Id,
                        FriendId = lastFriendMessages.FriendId,
                        IsBlocked = true
                    });
                }
            }
        }

        public void SendMessageToUnanswered(long senderId, long friendId)
        {
            var message = String.Empty;

            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = senderId
            });
            var friend = new GetFriendByIdFacebookQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdFacebookQuery
            {
                FacebookId = friendId
            });

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

            if (lastFriendMessages != null && lastBotMessages != null)
            {
                var orderNumber = lastBotMessages.OrderNumber + 1;

                var messageModel = GetRandomMessage(messageData, orderNumber);
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
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.SendMessage
                    })
                });

                new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
                {
                    AccountId = account.Id,
                    FriendId = friendId,
                    OrderNumber = orderNumber,
                    Message = message,
                    MessageDateTime = DateTime.Now
                });

                if (messageData != null && orderNumber >= numberLastResponseMessage)
                {
                    new MarkBlockedFriendCommandHandler(new DataBaseContext()).Handle(new MarkBlockedFriendCommand()
                    {
                        AccountId = account.Id,
                        FriendId = lastFriendMessages.FriendId,
                        IsBlocked = true
                    });
                }
            }
        }

        public void SendMessageToNewFriend(long senderId, long friendId)
        {
            var message = String.Empty;

            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = senderId
            });

            var messageData = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
            {
                AccountId = account.Id
            }).Where(model => model.MessageRegime == MessageRegime.BotFirstMessage && model.OrderNumber == 1).ToList();

            var messageModel = GetRandomMessage(messageData, 1);
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
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.SendMessage
                })
            });

            new SaveSentMessageCommandHandler(new DataBaseContext()).Handle(new SaveSentMessageCommand()
            {
                AccountId = account.Id,
                FriendId = friendId,
                OrderNumber = 1,
                Message = message,
                MessageDateTime = DateTime.Now
            });
        }


        private MessageModel GetRandomMessage(IEnumerable<MessageModel> messages, int orderNumber)
        {
            return messages.Where(model => model.OrderNumber == orderNumber).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
