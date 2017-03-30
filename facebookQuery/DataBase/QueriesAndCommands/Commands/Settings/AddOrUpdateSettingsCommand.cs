using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Commands.Settings
{
    public class AddOrUpdateSettingsCommand : ICommand<long>
    {
        public long GroupId { get; set; }

        public GeoOptionsDbModel GeoOptions { get; set; }

        public MessageOptionsDbModel MessageOptions { get; set; }

        public FriendOptionsDbModel FriendsOptions { get; set; }

        public LimitsOptionsDbModel LimitsOptions { get; set; }

        public CommunityOptionsDbModel CommunityOptions { get; set; }

        public DeleteFriendsOptionsDbModel DeleteFriendsOptions { get; set; }

        public WinkFriendsOptionsDbModel WinkFriendsOptions { get; set; }
    }
}
