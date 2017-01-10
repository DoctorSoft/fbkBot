using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.SpyFunctions
{
    public class GetSpyFunctionByIdQueryHandler : IQueryHandler<GetSpyFunctionByIdQuery, SpyFunctionData>
    {
        private readonly DataBaseContext context;

        public GetSpyFunctionByIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public SpyFunctionData Handle(GetSpyFunctionByIdQuery query)
        {
            var result = context.SpyFunctions.Select(model => new SpyFunctionData
            {
                Name = model.Function.Name,
                FunctionName = model.Function.FunctionName,
                FunctionId = model.Function.Id,
                SpyId = model.Spy.Id,
            }).FirstOrDefault(data => data.FunctionName == query.FunctionName && data.SpyId == query.SpyId);

            return result;
        }
    }
}
