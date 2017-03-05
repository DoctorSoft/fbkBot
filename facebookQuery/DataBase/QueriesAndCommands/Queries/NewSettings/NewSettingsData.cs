using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Queries.NewSettings
{
    public class NewSettingsData
    {
        public long Id { get; set; }

        public long SettingsGroupId { get; set; }

        public long AccountId { get; set; }

        public CommunityOptionsDbModel CommunityOptions { get; set; }
    }
}
