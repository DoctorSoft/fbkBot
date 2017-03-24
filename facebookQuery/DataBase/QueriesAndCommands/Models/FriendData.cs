using System;
using Constants.GendersUnums;
using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Models
{
    public class FriendData
    {
        public long Id { get; set; }
        
        public long FacebookId { get; set; }

        public long AccountId { get; set; }

        public GenderEnum Gender { get; set; }

        public DateTime AddedDateTime { get; set; }

        public string Href { get; set; }

        public bool Deleted { get; set; }
        
        public string FriendName { get; set; }

        public bool DialogIsCompleted { get; set; }

        public bool IsAddedToGroups { get; set; }

        public bool IsAddedToPages { get; set; }

        public bool IsWinked { get; set; }
        
        public MessageRegime? MessageRegime { get; set; }
    }
}
