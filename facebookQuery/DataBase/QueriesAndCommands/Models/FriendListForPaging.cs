using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Models
{
    public class FriendListForPaging
    {
        public List<FriendData> Friends { get; set; }

        public int CountAllFriends { get; set; }
    }
}
