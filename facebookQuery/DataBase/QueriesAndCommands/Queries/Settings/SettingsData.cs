using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Queries.Settings
{
    public class SettingsData
    {
        public long Id { get; set; }

        public long GroupId { get; set; }

        public GeoOptionsDbModel GeoOptions { get; set; }

        public MessageOptionsDbModel MessageOptions { get; set; }

        public FriendOptionsDbModel FriendsOptions { get; set; }

        public LimitsOptionsDbModel LimitsOptions { get; set; }

        public CommunityOptionsDbModel CommunityOptions { get; set; }
    }
}
