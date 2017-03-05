using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand
{
    public class ChangeAnalysisFriendStatusCommandHandler : ICommandHandler<ChangeAnalysisFriendStatusCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public ChangeAnalysisFriendStatusCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(ChangeAnalysisFriendStatusCommand command)
        {
            var friendModel = context.AnalisysFriends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.FacebookId == command.FriendFacebookId);

            if (friendModel != null)
            {
                friendModel.Status = command.NewStatus;
            }

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
