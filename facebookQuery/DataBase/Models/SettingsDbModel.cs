﻿namespace DataBase.Models
{
    public class SettingsDbModel
    {
        public long Id { get; set; }

        public GroupSettingsDbModel SettingsGroup { get; set; }
        
        public string GeoOptions { get; set; }
        
        public string MessageOptions { get; set; }

        public string FriendsOptions { get; set; }

        public string LimitsOptions { get; set; }

        public string CommunityOptions { get; set; }

        public string DeleteFriendsOptions { get; set; }

        public string WinkFriendsOptions { get; set; }
    }
}
