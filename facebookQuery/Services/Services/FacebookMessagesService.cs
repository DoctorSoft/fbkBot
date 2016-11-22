using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Constants.FriendsEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands;
using DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand;
using DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Message;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Engines.Engines.GetMessagesEngine.GetMessages;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Engines.Engines.SendMessageEngine;
using Services.ViewModels.FriendMessagesModels;
using Services.ViewModels.MessagesModels;
using UnreadMessageModel = Services.ViewModels.MessagesModels.UnreadMessageModel;

namespace Services.Services
{
    public class FacebookMessagesService
    {
        public void SendMessageCore(long senderId, long friendId)
        {
            var message = "";

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = senderId
            });

            var friendMessageData = new GetFriendMessagesQueryHandler(new DataBaseContext()).Handle(new GetFriendMessagesQuery()
            {
                AccountId = senderId,
                FriendId = friendId
            }).OrderByDescending(data => data.OrderNumber).FirstOrDefault();

            var messageData = new GetMessageModelQueryHandler(new DataBaseContext()).Handle(new GetMessageModelQuery()
            {
                AccountId = account.Id
            }).OrderByDescending(data => data.OrderNumber).FirstOrDefault();
            
            if (friendMessageData != null)
            {
                var orderNumber = friendMessageData.OrderNumber;

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
                        FriendId = friendMessageData.FriendId,
                        BlockedCause = FriendBlockedStatus.CorrespondenceEnd
                    });
                }
            }
        }

        public void SendMessage(long accountId)
        {
            var unreadMessagesList = GetUnreadMessages(accountId).UnreadMessages;
            foreach (var unreadMessage in unreadMessagesList)
            {
                SendMessageCore(accountId, unreadMessage.FriendId);
            }
        }

        public UnreadFriendMessageList GetUnreadMessages(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            var unreadMessages = new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel()
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                })
            });

            if (unreadMessages.Count != 0)
            {
                new SaveUnreadMessagesCommandHandler(new DataBaseContext()).Handle(new SaveUnreadMessagesCommand()
                {
                    AccountId = account.Id,
                    UnreadMessages = unreadMessages
                });
                
                foreach (var unreadMessage in unreadMessages)
                {
                    new ChangeMessageStatusEngine().Execute(new ChangeMessageStatusModel()
                    {
                        AccountId = account.UserId,
                        FriendId = unreadMessage.FriendId,
                        Cookie = account.Cookie.CookieString,
                        UrlParameters =  new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                        }),
                    });

                    Thread.Sleep(2000);
                }
            }
            return new UnreadFriendMessageList()
            {
                UnreadMessages = unreadMessages.Select(model => new UnreadFriendMessageModel
                {
                    FriendId = model.FriendId,
                    UnreadMessage = model.UnreadMessage,
                    LastMessage = model.LastMessage,
                    CountAllMessages = model.CountAllMessages,
                    CountUnreadMessages = model.CountUnreadMessages,
                    LastReadMessageDateTime = model.LastReadMessageDateTime,
                    LastUnreadMessageDateTime = model.LastUnreadMessageDateTime
                }).ToList()
            };
        }

        public UnreadMessagesListViewModel GetUnreadMessagesFromAccountPage(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            var getUnreadMessagesUrlParameters =
                new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.GetUnreadMessages
                });

            var unreadMessagesList =  new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel()
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                UrlParameters = getUnreadMessagesUrlParameters
            });

            return new UnreadMessagesListViewModel()
            {
                UnreadMessages = unreadMessagesList.Select(model=> new UnreadMessageModel
                {
                    LastMessage = model.LastMessage,
                    UnreadMessage = model.UnreadMessage,
                    CountAllMessages = model.CountAllMessages,
                    CountUnreadMessages = model.CountUnreadMessages,
                    FacebookFriendId = model.FriendId
                }).ToList()
            };
        }


        public List<GetMessagesResponseModel> GetAllMessages(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });
            return new GetMessagesEngine().Execute(new GetMessagesModel()
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString
            });
        }

        public void GetСorrespondenceByFriendId(long accountId, long friendId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
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
                UrlParameters = urlParameters
            });
        }
    }
}
