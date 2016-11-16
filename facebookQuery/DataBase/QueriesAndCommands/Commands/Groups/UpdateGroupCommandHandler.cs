using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class UpdateGroupCommandHandler : ICommandHandler<UpdateGroupCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateGroupCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(UpdateGroupCommand command)
        {
            new RemoveGroupCommandHandler(context).Handle(new RemoveGroupCommand
            {
                Id = command.Id
            });

            new AddNewGroupCommandHandler(context).Handle(new AddNewGroupCommand
            {
                Name = command.Name
            });

            return new VoidCommandResponse();
        }
    }
}
