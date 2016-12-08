using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.ExtraMessages
{
    public class UpdateExtraMessageCommandHandler : ICommandHandler<UpdateExtraMessageCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateExtraMessageCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(UpdateExtraMessageCommand command)
        {
            var updatingGroup = context.ExtraMessages.FirstOrDefault(model => model.Id == command.Id);
 
            if (context.ExtraMessages.Any(model => model.Message.ToUpper() == command.Name.ToUpper() && model.Id != command.Id))
            {
                return new VoidCommandResponse();
            }

            updatingGroup.Message = command.Name;

            context.ExtraMessages.AddOrUpdate(updatingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
