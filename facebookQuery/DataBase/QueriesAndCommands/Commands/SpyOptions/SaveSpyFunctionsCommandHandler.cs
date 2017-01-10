using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.SpyOptions
{
    public class SaveSpyFunctionsCommandHandler : ICommandHandler<SaveSpyFunctionsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveSpyFunctionsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SaveSpyFunctionsCommand command)
        {
            context.SpyFunctions.Where(model => model.SpyId == command.SpyId).Delete();

            if (command.Functions == null || !command.Functions.Any())
            {
                context.SaveChanges();
                return new VoidCommandResponse();
            }

            context.BulkInsert(command.Functions.Select(functionId => new SpyFunctionDbModel
            {
                FunctionId = functionId,
                SpyId = command.SpyId
            }));

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
