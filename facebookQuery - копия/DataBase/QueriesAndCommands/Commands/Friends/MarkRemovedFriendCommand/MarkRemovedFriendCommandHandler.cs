using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkRemovedFriendCommand
{
    public class MarkRemovedFriendCommandHandler : ICommandHandler<MarkRemovedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public MarkRemovedFriendCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(MarkRemovedFriendCommand command)
        {
            var friendModel = context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.DeleteFromFriends = true;
            
            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
