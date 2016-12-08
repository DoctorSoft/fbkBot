using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.ExtraMessages
{
    public class AddNewExtraMessagesCommandHandler : ICommandHandler<AddNewExtraMessagesCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddNewExtraMessagesCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(AddNewExtraMessagesCommand command)
        {
            if (context.ExtraMessages.Any(model => model.Message.ToUpper() == command.Name.ToUpper()))
            {
                return new VoidCommandResponse();
            }

            var group = new ExtraMessageDbModel
            {
                Message = command.Name
            };

            context.ExtraMessages.Add(group);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
