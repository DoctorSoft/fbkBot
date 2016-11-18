using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;

namespace DataBase.QueriesAndCommands.Commands.StopWords
{
    public class UpdateStopWordCommandHandler : ICommandHandler<UpdateStopWordCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateStopWordCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(UpdateStopWordCommand command)
        {
            var updatingGroup = context.StopWords.FirstOrDefault(model => model.Id == command.Id);
 
            if (context.StopWords.Any(model => model.Word.ToUpper() == command.Name.ToUpper() && model.Id != command.Id))
            {
                return new VoidCommandResponse();
            }

            updatingGroup.Word = command.Name;

            context.StopWords.AddOrUpdate(updatingGroup);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
