namespace DataBase.QueriesAndCommands.Models.JsonModels
{
    public class DeleteFriendsOptionsDbModel
    {
        public bool EnableDialogIsOver { get; set; }

        public bool EnableIsAddedToGroupsAndPages { get; set; }

        public bool EnableIsWink { get; set; }

        public bool EnableIsWinkFriendsOfFriends { get; set; }

        public int DialogIsOverTimer { get; set; }

        public int IsAddedToGroupsAndPagesTimer { get; set; }

        public int IsWinkTimer { get; set; }

        public int IsWinkFriendsOfFriendsTimer { get; set; }
    }
}
