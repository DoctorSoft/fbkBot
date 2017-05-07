using System;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveUnreadMessagesCommand
{
    public class SaveUnreadMessagesCommandHandler : ICommandHandler<SaveUnreadMessagesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public SaveUnreadMessagesCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }
        public VoidCommandResponse Handle(SaveUnreadMessagesCommand command)
        {
            if (!command.UnreadMessages.Any())
            {
                return new VoidCommandResponse();
            }

            foreach (var unreadMessageInformation in command.UnreadMessages)
            {
                var friendId = unreadMessageInformation.FriendFacebookId;

                var friend =
                    _context.Friends
                    .FirstOrDefault(model => model.AccountId == command.AccountId 
                        && model.FacebookId.Equals(friendId)) ??
                    new FriendDbModel
                        {
                            AccountId = command.AccountId,
                            FacebookId = unreadMessageInformation.FriendFacebookId,
                            MessageRegime = MessageRegime.UserFirstMessage,
                            Gender = unreadMessageInformation.Gender,
                            Href = unreadMessageInformation.Href,
                            AddedDateTime = new DateTime(2000,01,01),
                            FriendName = unreadMessageInformation.Name,
                            FriendType = unreadMessageInformation.FriendType
                        };

                if (_context.FriendsBlackList.Any(model => model.FriendFacebookId == friend.FacebookId && model.GroupId == _context.Accounts.FirstOrDefault(dbModel => dbModel.Id == command.AccountId).GroupSettingsId || friend.DeleteFromFriends))
                {
                    continue;
                }

                var lastBotMessage =
                    _context.FriendMessages.OrderByDescending(model => model.MessageDateTime)
                        .FirstOrDefault(
                            model =>
                                model.FriendId.Equals(friend.Id)
                                && model.MessageDirection == MessageDirection.ToFriend
                                && model.Friend.AccountId.Equals(command.AccountId));

                if (lastBotMessage == null)
                {
                    friend.MessageRegime = MessageRegime.UserFirstMessage;

                    _context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = unreadMessageInformation.FriendFacebookId,
                        MessageDirection = MessageDirection.FromFriend,
                        Message = unreadMessageInformation.LastMessage,
                        MessageDateTime = DateTime.Now,
                        OrderNumber = 1,
                        Friend = friend
                    });

                    _context.SaveChanges();
                    continue;
                }

                var lastFriendMessage =
                    _context.FriendMessages.OrderByDescending(model => model.MessageDateTime)
                        .FirstOrDefault(
                            model =>
                                model.FriendId.Equals(friend.Id)
                                && model.MessageDirection == MessageDirection.FromFriend
                                && model.Friend.AccountId.Equals(command.AccountId));

                if (lastFriendMessage == null)
                {
                    _context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = unreadMessageInformation.FriendFacebookId,
                        MessageDirection = MessageDirection.FromFriend,
                        Message = unreadMessageInformation.LastMessage,
                        MessageDateTime = DateTime.Now,
                        OrderNumber = 1,
                        Friend = friend
                    });

                    _context.SaveChanges();
                    continue;
                }

                if (lastBotMessage.OrderNumber == lastFriendMessage.OrderNumber)
                {
                    if (unreadMessageInformation.LastUnreadMessageDateTime != lastFriendMessage.MessageDateTime)
                    {
                        _context.FriendMessages.Add(new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendFacebookId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = lastBotMessage.OrderNumber + 1,
                            Friend = friend
                        });
                    }

                    _context.SaveChanges();
                    continue;
                }
                if (lastBotMessage.OrderNumber > lastFriendMessage.OrderNumber)
                {
                    if (unreadMessageInformation.LastUnreadMessageDateTime > lastFriendMessage.MessageDateTime)
                    {
                        _context.FriendMessages.Add(new FriendMessageDbModel
                        {
                            FriendId = unreadMessageInformation.FriendFacebookId,
                            MessageDirection = MessageDirection.FromFriend,
                            Message = unreadMessageInformation.LastMessage,
                            MessageDateTime = unreadMessageInformation.LastUnreadMessageDateTime,
                            OrderNumber = lastBotMessage.OrderNumber,
                            Friend = friend
                        });
                    }
                    _context.SaveChanges();
                   continue;
                }
            }

            return new VoidCommandResponse();
        }
    }
}
