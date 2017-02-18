using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.DeleteFriendByIdCommand
{
    public class DeleteFriendByIdCommandHandler : ICommandHandler<DeleteFriendByIdCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public DeleteFriendByIdCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(DeleteFriendByIdCommand command)
        {
            var friendModel = context.Friends.FirstOrDefault(model => model.AccountId == command.AccountId && model.Id == command.FriendId);

            if (friendModel == null)
            {
                return new VoidCommandResponse();
            }

            context.Friends.Remove(friendModel);
            context.SaveChanges();
            return new VoidCommandResponse();
        }
    }
}
