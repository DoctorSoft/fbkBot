using System;
using Constants.FriendTypesEnum;
using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class AnalysisFriendsData
    {
        public long Id { get; set; }

        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public long AccountId { get; set; }
        
        public DateTime AddedDateTime { get; set; }

        public StatusesFriend Status { get; set; }

        public FriendTypes Type { get; set; }
    }
}
