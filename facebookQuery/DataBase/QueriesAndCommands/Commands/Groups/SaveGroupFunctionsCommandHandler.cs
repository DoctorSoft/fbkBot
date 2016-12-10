using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class SaveGroupFunctionsCommandHandler : ICommandHandler<SaveGroupFunctionsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public SaveGroupFunctionsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(SaveGroupFunctionsCommand command)
        {
            context.GroupFunctions.Where(model => model.GroupId == command.GroupId).Delete();

            if (command.Functions == null || !command.Functions.Any())
            {
                context.SaveChanges();
                return new VoidCommandResponse();
            }

            context.BulkInsert(command.Functions.Select(functionId => new GroupFunctionDbModel
            {
                FunctionId = functionId,
                GroupId = command.GroupId
            }));

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
