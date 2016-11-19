using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Links
{
    public class AddNewLinkCommandHandler : ICommandHandler<AddNewLinkCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddNewLinkCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(AddNewLinkCommand command)
        {
            if (context.Links.Any(model => model.Link.ToUpper() == command.Name.ToUpper()))
            {
                return new VoidCommandResponse();
            }

            var link = new LinkDbModel
            {
                Link = command.Name
            };

            context.Links.Add(link);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
