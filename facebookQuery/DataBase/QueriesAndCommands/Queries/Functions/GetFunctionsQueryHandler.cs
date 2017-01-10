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
            var result = context
            .Functions
            .OrderBy(model => model.FunctionType.FunctionTypeName)
            .Select(model => new FunctionData
            {
                Name = model.Name,
                FunctionId = model.Id,
                FunctionName = model.FunctionName,
                FunctionTypeName = model.FunctionType.FunctionTypeName,
                TypeName = model.FunctionType.TypeName,
                ForSpy = model.ForSpy
            }).ToList();

            return query.ForSpy ? result.Where(data => data.ForSpy).ToList() : result.Where(data => !data.ForSpy).ToList();
        }
    }
}
