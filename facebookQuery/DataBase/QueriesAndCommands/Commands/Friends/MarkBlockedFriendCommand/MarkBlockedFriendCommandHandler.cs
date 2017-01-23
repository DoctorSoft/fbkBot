using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand
{
    public class MarkBlockedFriendCommandHandler : ICommandHandler<MarkBlockedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public MarkBlockedFriendCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(MarkBlockedFriendCommand command)
        {
            var friendModel = context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.IsBlocked = true;
            
            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
