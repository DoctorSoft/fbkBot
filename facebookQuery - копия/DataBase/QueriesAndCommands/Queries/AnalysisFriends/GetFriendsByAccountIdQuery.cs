using System.Collections.Generic;
using Constants.FriendTypesEnum;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class GetFriendsByAccountIdQuery : IQuery<List<AnalysisFriendsData>>
    {
        public long AccountId { get; set; }

        public StatusesFriend? Status { get; set; }

        public FriendTypes? FriendsType { get; set; }
    }
}
