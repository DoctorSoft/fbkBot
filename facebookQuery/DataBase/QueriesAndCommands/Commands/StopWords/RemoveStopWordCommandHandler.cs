using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;

namespace DataBase.QueriesAndCommands.Commands.StopWords
{
    public class RemoveStopWordCommandHandler : ICommandHandler<RemoveStopWordCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RemoveStopWordCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RemoveStopWordCommand command)
        {
            var delitingGroup = context.StopWords.FirstOrDefault(model => model.Id == command.Id);

            context.StopWords.Remove(delitingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
