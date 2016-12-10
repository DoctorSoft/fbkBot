using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Functions
{
    public class GetFunctionsQueryHandler : IQueryHandler<GetFunctionsQuery, List<FunctionData>>
    {
        private readonly DataBaseContext context;

        public GetFunctionsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FunctionData> Handle(GetFunctionsQuery query)
        {
            var result = context.Functions.Select(model => new FunctionData
            {
                Name = model.Name,
                FunctionId = model.Id,
                FunctionName = model.FunctionName
            }).ToList();

            return result;
        }
    }
}
