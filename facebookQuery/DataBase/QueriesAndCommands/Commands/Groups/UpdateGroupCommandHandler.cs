using System.Data.Entity.Migrations;
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
            var updatingGroup = context.GroupSettings.FirstOrDefault(model => model.Id == command.Id);
 
            if (context.GroupSettings.Any(model => model.Name.ToUpper() == command.Name.ToUpper() && model.Id != command.Id))
            {
                return new VoidCommandResponse();
            }

            updatingGroup.Name = command.Name;

            context.GroupSettings.AddOrUpdate(updatingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
