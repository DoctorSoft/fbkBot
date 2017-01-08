using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendTypeCommand
{
    public class ChangeAnalysisFriendTypeCommandHandler : ICommandHandler<ChangeAnalysisFriendTypeCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public ChangeAnalysisFriendTypeCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(ChangeAnalysisFriendTypeCommand command)
        {
            var friendModel = context.AnalisysFriends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.FacebookId == command.FriendFacebookId);

            if (friendModel != null)
            {
                friendModel.Type = command.NewType;
            }

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
