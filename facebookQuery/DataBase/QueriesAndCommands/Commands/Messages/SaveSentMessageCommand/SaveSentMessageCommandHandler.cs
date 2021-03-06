﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand
{
    public class SaveSentMessageCommandHandler : ICommandHandler<SaveSentMessageCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveSentMessageCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SaveSentMessageCommand command)
        {
            var friendId = command.FriendId;

            var account = context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);

            var friend =
                context.Friends.FirstOrDefault(
                    model => model.AccountId == command.AccountId && model.FacebookId.Equals(friendId));
            if (friend == null || context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == friend.FacebookId && dbModel.GroupId == account.GroupSettingsId) || friend.DeleteFromFriends)
            {
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
                context.FriendMessages.Add(new FriendMessageDbModel
                {
                    FriendId = friendId,
                    MessageDirection = MessageDirection.ToFriend,
                    Message = command.Message,
                    MessageDateTime = command.MessageDateTime,
                    OrderNumber = 1,
                    Friend = friend
                });

                context.SaveChanges();
                return new VoidCommandResponse();
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
                context.FriendMessages.Add(new FriendMessageDbModel
                {
                    FriendId = friendId,
                    MessageDirection = MessageDirection.ToFriend,
                    Message = command.Message,
                    MessageDateTime = command.MessageDateTime,
                    OrderNumber = 1,
                    Friend = friend
                });

                context.SaveChanges();
                return new VoidCommandResponse();
            }

            if (lastBotMessage.OrderNumber == lastFriendMessage.OrderNumber)
            {
                if (lastBotMessage.MessageDateTime > lastFriendMessage.MessageDateTime)
                {
                    //Extra - message
                    context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = friendId,
                        MessageDirection = MessageDirection.ToFriend,
                        Message = command.Message,
                        MessageDateTime = command.MessageDateTime,
                        OrderNumber = lastFriendMessage.OrderNumber,
                        Friend = friend
                    });
                }
                else
                {
                    context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = friendId,
                        MessageDirection = MessageDirection.ToFriend,
                        Message = command.Message,
                        MessageDateTime = command.MessageDateTime,
                        OrderNumber = lastFriendMessage.OrderNumber + 1,
                        Friend = friend
                    });
                }

                context.SaveChanges();
                return new VoidCommandResponse();
            }

            if (lastFriendMessage.OrderNumber < lastBotMessage.OrderNumber)
            {
                if (lastBotMessage.MessageDateTime > lastFriendMessage.MessageDateTime)
                {
                    //Extra - message
                    context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = friendId,
                        MessageDirection = MessageDirection.ToFriend,
                        Message = command.Message,
                        MessageDateTime = command.MessageDateTime,
                        OrderNumber = lastBotMessage.OrderNumber,
                        Friend = friend
                    });
                }
                else
                {
                    context.FriendMessages.Add(new FriendMessageDbModel
                    {
                        FriendId = friendId,
                        MessageDirection = MessageDirection.ToFriend,
                        Message = command.Message,
                        MessageDateTime = command.MessageDateTime,
                        OrderNumber = lastBotMessage.OrderNumber + 1,
                        Friend = friend
                    });
                }

                context.SaveChanges();
                return new VoidCommandResponse();
            }

            context.FriendMessages.Add(new FriendMessageDbModel
            {
                FriendId = friendId,
                MessageDirection = MessageDirection.ToFriend,
                Message = command.Message,
                MessageDateTime = command.MessageDateTime,
                OrderNumber = lastFriendMessage.OrderNumber,
                Friend = friend
            });

            context.SaveChanges();
            return new VoidCommandResponse();
        }
    }
}
