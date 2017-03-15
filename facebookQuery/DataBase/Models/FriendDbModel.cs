using System;
using System.Collections.Generic;
using Constants.GendersUnums;
using Constants.MessageEnums;

namespace DataBase.Models
{
    public class FriendDbModel
    {
        public long Id { get; set; }

        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public bool DeleteFromFriends { get; set; }
        
        public long AccountId { get; set; }

        public GenderEnum Gender { get; set; }

        public string Href { get; set; }
        
        public DateTime AddedDateTime { get; set; }

        public MessageRegime? MessageRegime { get; set; }

        public AccountDbModel AccountWithFriend { get; set; }

        public ICollection<FriendMessageDbModel> FriendMessages { get; set; }

        public bool DialogIsCompleted { get; set; }

        public bool IsAddedToGroups { get; set; }

        public bool IsAddedToPages { get; set; }

        public bool IsWinked { get; set; }
    }
}
