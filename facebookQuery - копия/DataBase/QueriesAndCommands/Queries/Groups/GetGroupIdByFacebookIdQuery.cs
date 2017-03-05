using System.Runtime.CompilerServices;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupIdByFacebookIdQuery : IQuery<long?>
    {
        public long FacebookId { get; set; }
    }
}
