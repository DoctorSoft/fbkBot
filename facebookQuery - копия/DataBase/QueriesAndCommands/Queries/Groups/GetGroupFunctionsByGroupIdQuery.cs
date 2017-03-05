using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupFunctionsByGroupIdQuery : IQuery<List<GroupFunctionData>>
    {
        public long GroupId { get; set; }
    }
}
