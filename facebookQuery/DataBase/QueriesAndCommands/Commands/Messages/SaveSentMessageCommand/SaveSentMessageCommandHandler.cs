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
            var friend = context.Friends.FirstOrDefault(model => model.AccountId == command.AccountId && model.IsBlocked == false && model.FacebookId.Equals(command.FriendId));

            if (friend!=null)
            {
                friend.FriendMessages = new Collection<FriendMessageDbModel>()
                {
                    new FriendMessageDbModel
                    {
                        FriendId = command.FriendId,
                        MessageDirection = MessageDirection.ToFriend,
                        Message = command.Message,
                        MessageDateTime = command.MessageDateTime,
                        OrderNumber = command.OrderNumber
                    }
                };
                context.SaveChanges();
            }
            return new VoidCommandResponse();
        }
    }
}
