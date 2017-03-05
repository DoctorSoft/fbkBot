using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class AddNewGroupCommandHandler : ICommandHandler<AddNewGroupCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddNewGroupCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(AddNewGroupCommand command)
        {
            if (context.GroupSettings.Any(model => model.Name.ToUpper() == command.Name.ToUpper()))
            {
                return new VoidCommandResponse();
            }

            var group = new GroupSettingsDbModel
            {
                Name = command.Name
            };

            context.GroupSettings.Add(group);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
