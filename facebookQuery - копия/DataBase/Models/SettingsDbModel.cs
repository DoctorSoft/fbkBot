namespace DataBase.Models
{
    public class SettingsDbModel
    {
        public long Id { get; set; }

        public GroupSettingsDbModel SettingsGroup { get; set; }
        
        public string GeoOptions { get; set; }
        
        public string MessageOptions { get; set; }

        public string FriendsOptions { get; set; }

        public string LimitsOptions { get; set; }
    }
}
