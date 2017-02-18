using System;

namespace DataBase.QueriesAndCommands.Models
{
    public class BlockedFriendData
    {
        public long Id { get; set; }
        
        public long FacebookId { get; set; }

        public string FriendName { get; set; }

        public DateTime DateAdded { get; set; }

        public long GroupId { get; set; }
    }
}
