using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendCommand
{
    public class MarkWinkedFriendCommandHandler : ICommandHandler<MarkWinkedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkWinkedFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkWinkedFriendCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.IsWinked = true;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
