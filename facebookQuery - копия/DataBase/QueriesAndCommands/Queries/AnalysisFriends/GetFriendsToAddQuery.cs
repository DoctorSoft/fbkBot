using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class GetFriendsToAddQuery : IQuery<List<AnalysisFriendsData>>
    {
        public long AccountId { get; set; }
    }
}
