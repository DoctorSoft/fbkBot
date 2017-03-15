using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class CommunityOptionsDbModel
    {
        public bool IsJoinToAllGroups { get; set; }

        public TimeModel RetryTimeInviteTheGroups { get; set; }

        public TimeModel RetryTimeInviteThePages { get; set; }

        public int MinFriendsJoinGroupInDay { get; set; }

        public int MaxFriendsJoinGroupInDay { get; set; }

        public int MinFriendsJoinGroupInHour { get; set; }

        public int MaxFriendsJoinGroupInHour { get; set; }

        public int MinFriendsJoinPageInDay { get; set; }

        public int MaxFriendsJoinPageInDay { get; set; }

        public int MinFriendsJoinPageInHour { get; set; }

        public int MaxFriendsJoinPageInHour { get; set; }

        public List<string> Groups { get; set; }

        public List<string> Pages { get; set; }
    }
}
