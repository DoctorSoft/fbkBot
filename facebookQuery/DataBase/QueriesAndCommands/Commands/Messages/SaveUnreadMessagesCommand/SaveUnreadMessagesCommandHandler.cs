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
            var firstAccount = context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);
            if (firstAccount != null)
            {
                accountId = firstAccount.Id;
            }

            if (!command.UnreadMessages.Any())
            {
                return new VoidCommandResponse();
            }

            foreach (var unreadMessageInformation in command.UnreadMessages)
            {
                var friendId = unreadMessageInformation.FriendId;

                var friend = context.Friends.FirstOrDefault(model => model.AccountId == accountId &&
                                                                     model.FriendId.Equals(friendId.ToString()));

                if (friend.IsBlocked)
                {
                    continue;
                }
                var isDuplicate = context.FriendMessages.Where(model => model.FriendId.Equals(friend.Id))
                    .Any(model => model.MessageDateTime.Equals(unreadMessageInformation.LastReadMessageDateTime));

                if (isDuplicate)
                {
                    continue;
                }

                var friendsMessagesInDb =
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

                if (friend == null)
                {
                    break;
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
                };
            }

            context.SaveChanges();
            return new VoidCommandResponse();
        }
    }
}
