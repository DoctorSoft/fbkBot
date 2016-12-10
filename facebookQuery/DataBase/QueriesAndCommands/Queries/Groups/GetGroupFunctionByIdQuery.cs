using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupFunctionByIdQuery : IQuery<GroupFunctionData>
    {
        public long GroupId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
