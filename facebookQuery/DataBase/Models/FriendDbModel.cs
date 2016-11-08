using System.Collections.Generic;

namespace DataBase.Models
{
    public class FriendDbModel
    {
        public long Id { get; set; }

        public string FriendId { get; set; }

        public string FriendName { get; set; }

        public AccountDbModel AccountWithFriend { get; set; }

        public long AccountId { get; set; }

        public ICollection<FriendMessageDbModel> FriendMessages { get; set; } 
    }
}
