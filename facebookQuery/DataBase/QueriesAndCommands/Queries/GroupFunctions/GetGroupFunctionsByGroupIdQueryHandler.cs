using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.GroupFunctions
{
    public class GetGroupFunctionsByGroupIdQueryHandler : IQueryHandler<GetGroupFunctionsByGroupIdQuery, List<FunctionName>>
    {
        private readonly DataBaseContext _context;

        public GetGroupFunctionsByGroupIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FunctionName> Handle(GetGroupFunctionsByGroupIdQuery query)
        {
            var functionsIdList = _context
            .GroupFunctions
            .Where(model => model.GroupId == query.GroupId).Select(model => model.FunctionId).ToList();
            var result = functionsIdList.Select(functionId =>
            {
                var functionDbModel = _context.Functions.FirstOrDefault(model => model.Id == functionId);
                return functionDbModel != null ? functionDbModel.FunctionName : (FunctionName) 0;
            }).ToList();
            
            return result;
        }
    }
}
