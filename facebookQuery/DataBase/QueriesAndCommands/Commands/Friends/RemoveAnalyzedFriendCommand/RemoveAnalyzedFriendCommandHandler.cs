using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand
{
    public class RemoveAnalyzedFriendCommandHandler : ICommandHandler<RemoveAnalyzedFriendCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveAnalyzedFriendCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveAnalyzedFriendCommand command)
        {
            var friendModel = context.AnalisysFriends.FirstOrDefault(
                model => model.AccountId == command.AccountId && model.Id == command.FriendId);

            if (friendModel == null || friendModel.Status != StatusesFriend.ToDelete)
            {
                return new VoidCommandResponse();
            }

            context.Set<AnalysisFriendDbModel>().Remove(friendModel);
            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
