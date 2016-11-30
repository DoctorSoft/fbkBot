using System.Collections.ObjectModel;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommandHandler : ICommandHandler<SaveUnreadMessagesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveUnreadMessagesCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public VoidCommandResponse Handle(SaveUnreadMessagesCommand command)
        {
            if (!command.UnreadMessages.Any())
            {
                return new VoidCommandResponse();
            }

            foreach (var unreadMessageInformation in command.UnreadMessages)
            {
                var friendId = unreadMessageInformation.FriendId;
                
                var friend = context.Friends.FirstOrDefault(model => model.AccountId == command.AccountId && model.FacebookId.Equals(friendId));
                if (friend == null || friend.IsBlocked || friend.DeleteFromFriends)
                {
                    continue;
                }

                var lastBotMessage =
                    context.FriendMessages.OrderByDescending(model => model.MessageDateTime)
                        .FirstOrDefault(
                            model =>
                                model.FriendId.Equals(friend.Id) 
                                && model.MessageDirection == MessageDirection.ToFriend 
                                && model.Friend.AccountId.Equals(command.AccountId));

                if (lastBotMessage == null)
                {
                    friend.FriendMessages = new Collection<FriendMessageDbModel>()
                    {
                        new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = 1,
                            MessageRegime = MessageRegime.UserFirstMessage
                        }
                    };

                    context.SaveChanges();
                    return new VoidCommandResponse();
                }

                var lastFriendMessage =
                context.FriendMessages.OrderByDescending(model => model.MessageDateTime)
                    .FirstOrDefault(
                        model =>
                            model.FriendId.Equals(friend.Id)
                            && model.MessageDirection == MessageDirection.FromFriend
                            && model.Friend.AccountId.Equals(command.AccountId));

                if (lastFriendMessage == null)
                {
                    friend.FriendMessages = new Collection<FriendMessageDbModel>()
                    {
                        new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = 1,
                            MessageRegime = MessageRegime.BotFirstMessage
                        }
                    };

                    context.SaveChanges();
                    return new VoidCommandResponse();
                }

                if (lastBotMessage.OrderNumber == lastFriendMessage.OrderNumber)
                {
                    friend.FriendMessages = new Collection<FriendMessageDbModel>()
                    {
                        new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = lastBotMessage.OrderNumber++,
                            MessageRegime = MessageRegime.UserFirstMessage
                        }
                    };

                    context.SaveChanges();
                    return new VoidCommandResponse();
                }

                friend.FriendMessages = new Collection<FriendMessageDbModel>()
                    {
                        new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = lastBotMessage.OrderNumber,
                            MessageRegime = MessageRegime.BotFirstMessage
                        }
                    };

                context.SaveChanges();
                return new VoidCommandResponse();
                
/*                var friendsMessagesInDb =
                    context.FriendMessages.Where(model => model.FriendId == friend.Id).Select(model => new
                    {
                        model.OrderNumber
                    }).AsEnumerable().Select(model => new FriendMessageDbModel()
                    {
                        OrderNumber = model.OrderNumber
                    }).ToList();

                var friendMessageDbModel = friendsMessagesInDb.OrderByDescending(model => model.OrderNumber).FirstOrDefault();
                var orderNumberMessage = 0;
                if (friendMessageDbModel != null)
                {
                    orderNumberMessage = friendMessageDbModel.OrderNumber;
                }
                
                friend.FriendMessages = new Collection<FriendMessageDbModel>()
                {
                    new FriendMessageDbModel
                    {
                        FriendId = unreadMessageInformation.FriendId,
                        MessageDirection = MessageDirection.FromFriend,
                        Message = unreadMessageInformation.LastMessage,
                        MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                        OrderNumber = orderNumberMessage + 1
                    }
                };*/
            }

            return new VoidCommandResponse();
        }
    }
}
