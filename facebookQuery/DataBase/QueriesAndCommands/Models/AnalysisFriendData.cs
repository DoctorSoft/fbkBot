using System;
using Constants.FriendTypesEnum;
using Constants.GendersUnums;

namespace DataBase.QueriesAndCommands.Models
{
    public class AnalysisFriendData
    {
        public long Id { get; set; }
        
        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public long AccountId { get; set; }

        public string Uri { get; set; }

        public GenderEnum Gender { get; set; }
        
        public DateTime AddedToAnalysDateTime { get; set; }
        
        public StatusesFriend Status { get; set; }

        public FriendTypes Type { get; set; }
    }
}
