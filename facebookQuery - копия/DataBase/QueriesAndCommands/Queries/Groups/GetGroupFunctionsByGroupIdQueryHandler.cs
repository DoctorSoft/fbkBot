using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupFunctionsByGroupIdQueryHandler : IQueryHandler<GetGroupFunctionsByGroupIdQuery, List<GroupFunctionData>>
    {
        private readonly DataBaseContext _context;

        public GetGroupFunctionsByGroupIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<GroupFunctionData> Handle(GetGroupFunctionsByGroupIdQuery query)
        {
            var result = _context.GroupFunctions.Where(data => data.GroupId == query.GroupId)
                .Select(model => new GroupFunctionData
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
