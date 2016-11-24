using System;
using System.Collections.Generic;

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

        public DateTime AddedDateTime { get; set; }

        public AccountDbModel AccountWithFriend { get; set; }

        public ICollection<FriendMessageDbModel> FriendMessages { get; set; }
    }
}
