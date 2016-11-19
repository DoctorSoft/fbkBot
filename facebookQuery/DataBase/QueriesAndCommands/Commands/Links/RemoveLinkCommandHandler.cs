using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Links
{
    public class RemoveLinkCommandHandler : ICommandHandler<RemoveLinkCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveLinkCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveLinkCommand command)
        {
            var delitingLink = context.Links.FirstOrDefault(model => model.Id == command.Id);

            context.Links.Remove(delitingLink);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
