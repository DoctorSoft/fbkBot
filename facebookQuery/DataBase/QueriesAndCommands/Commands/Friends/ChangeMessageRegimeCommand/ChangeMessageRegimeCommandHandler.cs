using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeMessageRegimeCommand
{
    public class ChangeMessageRegimeCommandHandler : ICommandHandler<ChangeMessageRegimeCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public ChangeMessageRegimeCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(ChangeMessageRegimeCommand command)
        {
            var friendModel = context.Friends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);

            if (friendModel != null && friendModel.MessageRegime == null)
            {
                friendModel.MessageRegime = command.MessageRegime;
            }

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
