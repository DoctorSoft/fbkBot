using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Functions.GetFunctionName
{
    public class GetFunctionNameByNameQuery : IQuery<string>
    {
        public FunctionName FunctionName { get; set; }
    }
}
