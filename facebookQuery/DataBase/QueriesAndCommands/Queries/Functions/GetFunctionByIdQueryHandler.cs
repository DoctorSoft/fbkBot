using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Functions
{
    public class GetFunctionByIdQueryHandler : IQueryHandler<GetFunctionByIdQuery, FunctionData>
    {
        private readonly DataBaseContext _context;

        public GetFunctionByIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FunctionData Handle(GetFunctionByIdQuery query)
        {
            var result = _context
            .Functions
            .FirstOrDefault(model => model.Id == query.Id);

            if (result == null)
            {
                return null;
            }

            var functionData = new FunctionData
            {
                Name = result.Name,
                FunctionId = result.Id,
                FunctionName = result.FunctionName,
                FunctionTypeName = result.FunctionType.FunctionTypeName,
                TypeName = result.FunctionType.TypeName,
                ForSpy = result.ForSpy
            };

            return functionData;
        }
    }
}
