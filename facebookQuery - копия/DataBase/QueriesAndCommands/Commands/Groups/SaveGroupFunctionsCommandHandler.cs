using System.Linq;
using DataBase.Context;
using DataBase.Models;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;

namespace DataBase.QueriesAndCommands.Commands.Groups
{
    public class SaveGroupFunctionsCommandHandler : ICommandHandler<SaveGroupFunctionsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public SaveGroupFunctionsCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public VoidCommandResponse Handle(SaveGroupFunctionsCommand command)
        {
            _context.GroupFunctions.Where(model => model.GroupId == command.GroupId).Delete();

            if (command.Functions == null || !command.Functions.Any())
            {
                _context.SaveChanges();
                return new VoidCommandResponse();
            }

            _context.BulkInsert(command.Functions.Select(functionId => new GroupFunctionDbModel
            {
                FunctionId = functionId,
                GroupId = command.GroupId
            }));

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
