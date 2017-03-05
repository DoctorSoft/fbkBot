using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.SpyFunctions
{
    public class GetSpyFunctionByIdQuery : IQuery<SpyFunctionData>
    {
        public long SpyId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
