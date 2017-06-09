using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.GroupFunctions
{
    public class GetGroupFunctionsByGroupIdQuery : IQuery<List<FunctionName>>
    {
        public long GroupId { get; set; }
    }
}
