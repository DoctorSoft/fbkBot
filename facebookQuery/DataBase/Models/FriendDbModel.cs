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
        
        public bool IsBlocked { get; set; }

        public long AccountId { get; set; }

        public GenderEnum Gender { get; set; }

        public string Href { get; set; }
        
        public DateTime AddedDateTime { get; set; }

        public MessageRegime? MessageRegime { get; set; }

        public AccountDbModel AccountWithFriend { get; set; }

        public ICollection<FriendMessageDbModel> FriendMessages { get; set; }
    }
}
