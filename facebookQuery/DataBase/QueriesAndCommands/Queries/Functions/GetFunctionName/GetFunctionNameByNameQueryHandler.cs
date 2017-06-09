using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Functions.GetFunctionName
{
    public class GetFunctionNameByNameQueryHandler : IQueryHandler<GetFunctionNameByNameQuery, string>
    {
        private readonly DataBaseContext _context;

        public GetFunctionNameByNameQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public string Handle(GetFunctionNameByNameQuery query)
        {
            var result = _context
            .Functions
            .FirstOrDefault(model => model.FunctionName == query.FunctionName);

            if (result == null)
            {
                return string.Empty;
            }

            return result.Name;
        }
    }
}
