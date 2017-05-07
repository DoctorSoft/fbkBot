using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkWinksFriendsFriendsCommand
{
    public class MarkWinksFriendsFriendsCommandHandler : ICommandHandler<MarkWinksFriendsFriendsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkWinksFriendsFriendsCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkWinksFriendsFriendsCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
            {
                var countWinks = friendModel.CountWinksToFriends;
                friendModel.CountWinksToFriends = countWinks + 1;
            }
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
