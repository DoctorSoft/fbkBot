using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Commands.Groups;

namespace DataBase.QueriesAndCommands.Commands.StopWords
{
    public class AddNewStopWordCommandHandler : ICommandHandler<AddNewStopWordCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddNewStopWordCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(AddNewStopWordCommand command)
        {
            if (context.StopWords.Any(model => model.Word.ToUpper() == command.Name.ToUpper()))
            {
                return new VoidCommandResponse();
            }

            var group = new StopWordDbModel
            {
                Word = command.Name
            };

            context.StopWords.Add(group);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
