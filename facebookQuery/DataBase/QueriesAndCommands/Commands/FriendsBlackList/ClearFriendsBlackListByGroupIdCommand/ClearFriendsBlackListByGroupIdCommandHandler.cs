using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.FriendsBlackList.ClearFriendsBlackListByGroupIdCommand
{
    public class ClearFriendsBlackListByGroupIdCommandHandler : ICommandHandler<ClearFriendsBlackListByGroupIdCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public ClearFriendsBlackListByGroupIdCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(ClearFriendsBlackListByGroupIdCommand command)
        {
            context.FriendsBlackList.RemoveRange(context.FriendsBlackList.Where(model => model.GroupId == command.GroupSettingsId));

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
