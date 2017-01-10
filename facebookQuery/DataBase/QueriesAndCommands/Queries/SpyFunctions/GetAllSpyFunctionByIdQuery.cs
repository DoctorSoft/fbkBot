using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.SpyFunctions
{
    public class GetAllSpyFunctionByIdQuery : IQuery<List<SpyFunctionData>>
    {
        public long SpyId { get; set; }
    }
}
