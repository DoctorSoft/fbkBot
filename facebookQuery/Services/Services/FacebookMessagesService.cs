using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.FriendMessages;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetMessagesEngine.ChangeMessageStatus;
using Engines.Engines.GetMessagesEngine.GetMessages;
using Engines.Engines.GetMessagesEngine.GetUnreadMessages;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Services.Core;
using Services.ViewModels.FriendMessagesModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.MessagesModels;

namespace Services.Services
{
    public class FacebookMessagesService
    {
        public void SendMessageToUnread(AccountViewModel account)
        {
            var unreadMessagesList = GetUnreadMessages(account.FacebookId).UnreadMessages;
                        
            foreach (var unreadMessage in unreadMessagesList)
            {
                new SendMessageCore().SendMessageToUnread(account.FacebookId, unreadMessage.FriendId);
            }
        }

        public void SendMessageToUnanswered(AccountViewModel account)
        {
            var unansweredMessagesList = new GetUnansweredMessagesQueryHandler(new DataBaseContext()).Handle(new GetUnansweredMessagesQuery()
            {
                DelayTime = 2,
                AccountId = account.Id
            });

            foreach (var unansweredMessage in unansweredMessagesList)
            {
                var friend = new GetFriendByIdAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdAccountQuery
                {
                    AccountId = unansweredMessage.FriendId
                });

                new SendMessageCore().SendMessageToUnanswered(account.FacebookId, Convert.ToInt64(friend.FacebookId));
            }
        }

        public void SendMessageToNewFriends(AccountViewModel account)
        {
            var newFriends = new GetNewFriendsForDialogueQueryHandler(new DataBaseContext()).Handle(new GetNewFriendsForDialogueQuery()
            {
                DelayTime = 2,
                AccountId = account.Id
            });

//            foreach (var newFriend in newFriends)
//            {
//                new SendMessageCore().SendMessageToNewFriend(account.FacebookId, newFriend.FacebookId);
//
//                Thread.Sleep(3000);
//            }
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
