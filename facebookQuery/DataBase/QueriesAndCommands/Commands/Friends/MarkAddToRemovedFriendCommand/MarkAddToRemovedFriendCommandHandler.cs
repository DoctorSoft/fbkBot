using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToRemovedFriendCommand
{
    public class MarkAddToRemovedFriendCommandHandler : ICommandHandler<MarkAddToRemovedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkAddToRemovedFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkAddToRemovedFriendCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);
            if (friendModel != null)
                friendModel.AddedToRemoveDateTime = DateTime.Now;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
