using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToPageFriendCommand
{
    public class MarkAddToPageFriendCommandHandler : ICommandHandler<MarkAddToPageFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkAddToPageFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkAddToPageFriendCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.IsAddedToPages = true;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
