using System;
using System.Collections.Generic;
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
            var accountId = new long();
            var firstAccount = context.Accounts.FirstOrDefault(model => model.UserId == command.AccountId);
            if (firstAccount != null)
            {
                accountId = firstAccount.Id;
            }

            if (!command.UnreadMessages.Any()) return new VoidCommandResponse();

            var now = DateTime.Now;

            foreach (var unreadMessageInformation in command.UnreadMessages)
            {
                var friendId = unreadMessageInformation.FriendId.ToString();

                var friend = context.Friends.FirstOrDefault(model => model.AccountId == accountId &&
                                                                     model.FriendId.Equals(friendId));

                if (friend == null)
                {
                    break;
                }

                friend.FriendMessages = new Collection<FriendMessageDbModel>()
                {
                    new FriendMessageDbModel()
                    {
                        FriendId = unreadMessageInformation.FriendId,
                        MessageDirection = MessageDirection.FromFriend,
                        Message = unreadMessageInformation.LastMessage,
                        MessageDateTime = now
                    }
                };
            }

            context.SaveChanges();
            return new VoidCommandResponse();
        }
    }
}
