using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Links
{
    public class UpdateLinkCommandHandler : ICommandHandler<UpdateLinkCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateLinkCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(UpdateLinkCommand command)
        {
            var updatingLink = context.Links.FirstOrDefault(model => model.Id == command.Id);
 
            if (context.Links.Any(model => model.Link.ToUpper() == command.Name.ToUpper() && model.Id != command.Id))
            {
                return new VoidCommandResponse();
            }

            updatingLink.Link = command.Name;

            context.Links.AddOrUpdate(updatingLink);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
