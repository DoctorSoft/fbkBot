using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddedToRemoveTimeCommand
{
    public class MarkAddedToRemoveTimeCommandHandler : ICommandHandler<MarkAddedToRemoveTimeCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkAddedToRemoveTimeCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkAddedToRemoveTimeCommand command)
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
