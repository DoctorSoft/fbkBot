using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Messages
{
    public class RemoveMessageCommandHandler : ICommandHandler<RemoveMessageCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveMessageCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveMessageCommand command)
        {
            var messageToDelete = context.Set<MessageDbModel>().FirstOrDefault(model => model.Id == command.MessageId);

            context.Set<MessageDbModel>().Remove(messageToDelete);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
