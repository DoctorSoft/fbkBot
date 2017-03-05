using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupFunctionByIdQueryHandler : IQueryHandler<GetGroupFunctionByIdQuery, GroupFunctionData>
    {
        private readonly DataBaseContext context;

        public GetGroupFunctionByIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public GroupFunctionData Handle(GetGroupFunctionByIdQuery query)
        {
            var result = context.GroupFunctions.Select(model => new GroupFunctionData
            {
                Name = model.Function.Name,
                FunctionName = model.Function.FunctionName,
                FunctionId = model.Function.Id,
                GroupId = model.MessageGroup.Id,
                GroupName = model.MessageGroup.Name
            }).FirstOrDefault(data => data.FunctionName == query.FunctionName && data.GroupId == query.GroupId);

            return result;
        }
    }
}
