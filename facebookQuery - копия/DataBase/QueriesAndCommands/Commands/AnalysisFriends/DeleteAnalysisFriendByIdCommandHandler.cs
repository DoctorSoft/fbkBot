using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.AnalysisFriends
{
    public class DeleteAnalysisFriendByIdHandler : ICommandHandler<DeleteAnalysisFriendById, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public DeleteAnalysisFriendByIdHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(DeleteAnalysisFriendById command)
        {
            var account = context.AnalisysFriends.FirstOrDefault(model => model.FacebookId == command.AnalysisFriendFacebookId);

            context.AnalisysFriends.Remove(account);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
