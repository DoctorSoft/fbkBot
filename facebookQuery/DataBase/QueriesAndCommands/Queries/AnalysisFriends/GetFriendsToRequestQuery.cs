using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class GetFriendsToRequestQuery : IQuery<List<AnalysisFriendsData>>
    {
        public long AccountId { get; set; }
    }
}
