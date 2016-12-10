using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Queries.Functions;
using DataBase.QueriesAndCommands.Queries.Groups;
using Services.ViewModels.GroupFunctionsModels;

namespace Services.Services
{
    public class GroupFunctionsService
    {
        public List<GroupFunctionViewModel> GetGroupFunctions()
        {
            var functions = new GetFunctionsQueryHandler(new DataBaseContext()).Handle(new GetFunctionsQuery());
            var groups = new GetGroupsQueryHandler(new DataBaseContext()).Handle(new GetGroupsQuery());
            var groupFunctions = new GroupFunctionsQueryHandler(new DataBaseContext()).Handle(new GroupFunctionsQuery());

            var result = groups.Select(group => new GroupFunctionViewModel
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Functions = functions.Select(function => new FunctionViewModel
                {
                    Name = function.Name,
                    FunctionName = function.FunctionName,
                    FunctionId = function.FunctionId,
                    Assigned = groupFunctions.Any(groupFunction => groupFunction.FunctionId == function.FunctionId && groupFunction.GroupId == group.Id),
                    FunctionTypeName = function.FunctionTypeName,
                    TypeName = function.TypeName
                }).ToList()
            }).ToList();

            return result;
        }

        public void SaveGroupFunctions(long groupId, List<long> funtions)
        {
            new SaveGroupFunctionsCommandHandler(new DataBaseContext()).Handle(new SaveGroupFunctionsCommand
            {
                GroupId = groupId,
                Functions = funtions
            });
        }
    }
}
