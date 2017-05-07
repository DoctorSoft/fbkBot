using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand
{
    public class RemoveAnalyzedFriendCommandHandler : ICommandHandler<RemoveAnalyzedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public RemoveAnalyzedFriendCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(RemoveAnalyzedFriendCommand command)
        {
            var friendModel = _context.AnalisysFriends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);

            if (friendModel == null)
            {
                return new VoidCommandResponse();
            }

            _context.AnalisysFriends.Remove(friendModel);
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
