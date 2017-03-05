using System;

namespace DataBase.Models
{
    public class FriendsBlackListDbModel
    {
        public long Id { get; set; }

        public long FriendFacebookId { get; set; }

        public string FriendName { get; set; }

        public long GroupId { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
