using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.SendMessageEngine;

namespace Services.Core
{
    public class SendMessageCore
    {
        public void SendMessageToUnread(long senderId, long friendId)
        {
            var message = "";

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = senderId
            });

            var allMessages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = senderId,
                FriendId = friendId
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
            }).OrderByDescending(data => data.OrderNumber).FirstOrDefault();

            if (lastFriendMessages != null)
            {
                var orderNumber = lastFriendMessages.OrderNumber;

                var messageModel = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
                {
                    AccountId = account.Id
                }).FirstOrDefault(model => model.OrderNumber == orderNumber);
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

                if (messageData != null && orderNumber >= messageData.OrderNumber)
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
            var message = "";

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = senderId
            });

            var allMessages = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = senderId,
                FriendId = friendId
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
            }).OrderByDescending(data => data.OrderNumber).FirstOrDefault();

            if (lastFriendMessages != null)
            {
                var orderNumber = lastBotMessages.OrderNumber + 1;

                var messageModel = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
                {
                    AccountId = account.Id
                }).FirstOrDefault(model => model.OrderNumber == orderNumber);
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

                if (messageData != null && orderNumber >= messageData.OrderNumber)
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
    }
}
