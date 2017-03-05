using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class RemoveGroupCommandHandler : ICommandHandler<RemoveGroupCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveGroupCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveGroupCommand command)
        {
            var delitingGroup = context.GroupSettings.FirstOrDefault(model => model.Id == command.Id);

            context.GroupSettings.Remove(delitingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
