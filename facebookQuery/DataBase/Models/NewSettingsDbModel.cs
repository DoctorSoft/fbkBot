namespace DataBase.Models
{
    public class NewSettingsDbModel
    {
        public long Id { get; set; }

        public long SettingsGroupId { get; set; }

        public long AccountId { get; set; }

        public GroupSettingsDbModel SettingsGroup { get; set; }

        public AccountDbModel Account { get; set; }

        public string CommunityOptions { get; set; }
    }
}
