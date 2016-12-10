using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GroupFunctionsQueryHandler : IQueryHandler<GroupFunctionsQuery, List<GroupFunctionData>>
    {
        private readonly DataBaseContext context;

        public GroupFunctionsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<GroupFunctionData> Handle(GroupFunctionsQuery query)
        {
            var result = context.GroupFunctions.Select(model => new GroupFunctionData
            {
                Name = model.Function.Name,
                FunctionName = model.Function.FunctionName,
                FunctionId = model.Function.Id,
                GroupId = model.MessageGroup.Id,
                GroupName = model.MessageGroup.Name
            }).ToList();

            return result;
        }
    }
}
