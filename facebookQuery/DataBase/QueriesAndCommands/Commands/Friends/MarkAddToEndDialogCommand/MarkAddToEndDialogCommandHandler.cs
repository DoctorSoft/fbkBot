using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkAddToEndDialogCommand
{
    public class MarkAddToEndDialogCommandHandler : ICommandHandler<MarkAddToEndDialogCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkAddToEndDialogCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(MarkAddToEndDialogCommand command)
        {
            var friendModel = _context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.FacebookId == command.FriendId);
            if (friendModel != null)
                friendModel.DialogIsCompleted = true;
            
            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
