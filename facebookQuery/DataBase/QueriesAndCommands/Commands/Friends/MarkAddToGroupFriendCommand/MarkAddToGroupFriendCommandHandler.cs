using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToGroupFriendCommand
{
    public class MarkAddToGroupFriendCommandHandler : ICommandHandler<MarkAddToGroupFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkAddToGroupFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkAddToGroupFriendCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.IsAddedToGroups = true;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
