using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Engines.Engines.GetMessagesEngine.GetMessages;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Engines.Engines.SendMessageEngine;
using Services.ViewModels.MessagesModels;

namespace Services.Services
{
    public class FacebookMessagesService
    {
        public void SendMessage(long senderId, long friendId)
        {
            var rnd = new Random();
            var mockMessageList = new List<string>()
            {
                "Hello", "Hi", "How are u?", "What's up?", "What are you up to?", "Nice to meet you" 
            };
            var messageText = mockMessageList[rnd.Next(mockMessageList.Count)];

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = senderId
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.SendMessage
            });

            new SendMessageEngine().Execute(new SendMessageModel
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                FriendId = friendId,
                Message = messageText,
                UrlParameters = urlParameters
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

        public void GetUnreadMessages(long accountId)
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

            var unreadMessages = new GetUnreadMessagesEngine().Execute(new GetUnreadMessagesModel()
            {
                AccountId = account.UserId,
                Cookie = account.Cookie.CookieString,
                UrlParameters = getUnreadMessagesUrlParameters
            });

            new SaveUnreadMessagesCommandHandler(new DataBaseContext()).Handle(new SaveUnreadMessagesCommand()
            {
                AccountId = account.Id,
                UnreadMessages = unreadMessages
            });

            var changeMessageStatusUrlParameters =
                new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.ChangeMessageStatus
                });

            foreach (var unreadMessage in unreadMessages)
            {
                new ChangeMessageStatusEngine().Execute(new ChangeMessageStatusModel()
                {
                    UrlParameters = changeMessageStatusUrlParameters,
                    AccountId = account.Id,
                    FriendId = unreadMessage.FriendId,
                    Cookie = account.Cookie.CookieString
                });

                Thread.Sleep(2000);
            }

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
    }
}
