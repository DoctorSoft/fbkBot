using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.SpyFunctions
{
    public class GetAllSpyFunctionByIdQueryHandler : IQueryHandler<GetAllSpyFunctionByIdQuery, List<SpyFunctionData>>
    {
        private readonly DataBaseContext _context;

        public GetAllSpyFunctionByIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<SpyFunctionData> Handle(GetAllSpyFunctionByIdQuery query)
        {
            var result = _context.SpyFunctions.Select(model => new SpyFunctionData
            {
                Name = model.Function.Name,
                FunctionName = model.Function.FunctionName,
                FunctionId = model.Function.Id,
                SpyId = model.Spy.Id,
            }).Where(data => data.SpyId == query.SpyId).ToList();

            return result;
        }
    }
}
