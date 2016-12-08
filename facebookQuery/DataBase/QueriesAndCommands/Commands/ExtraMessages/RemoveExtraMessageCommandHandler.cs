using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.ExtraMessages
{
    public class RemoveExtraMessageCommandHandler : ICommandHandler<RemoveExtraMessageCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveExtraMessageCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveExtraMessageCommand command)
        {
            var delitingGroup = context.ExtraMessages.FirstOrDefault(model => model.Id == command.Id);

            context.ExtraMessages.Remove(delitingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
