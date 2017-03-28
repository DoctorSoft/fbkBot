using DataBase.QueriesAndCommands.Models.ConditionModels;

namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class DeleteFriendsOptionsDbModel
    {
        public DialogIsOverModel DialogIsOver { get; set; }

        public IsAddedToGroupsAndPagesModel IsAddedToGroupsAndPages { get; set; }

        public IsWinkModel IsWink { get; set; }

        public IsWinkFriendsOfFriendsModel IsWinkFriendsOfFriends { get; set; }

        public int DeletionFriendTimer { get; set; } 
    }
}
