using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendsFriendCommand
{
    public class MarkWinkedFriendsFriendCommandHandler : ICommandHandler<MarkWinkedFriendsFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkWinkedFriendsFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkWinkedFriendsFriendCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.IsWinkedFriendsFriend = true;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
