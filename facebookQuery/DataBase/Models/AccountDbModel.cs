using System.Collections.Generic;

namespace DataBase.Models
{
    public class AccountDbModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long FacebookId { get; set; }

        public string Proxy { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }

        public bool IsDeleted { get; set; }

        public long? GroupSettingsId { get; set; }

        public bool ProxyDataIsFailed { get; set; }

        public bool AuthorizationDataIsFailed { get; set; }

        public bool ConformationIsFailed { get; set; }

        public GroupSettingsDbModel GroupSettings { get; set; }

        public CookiesDbModel Cookies { get; set; }
        
        public ICollection<MessageDbModel> Messages { get; set; }

        public ICollection<FriendDbModel> Friends { get; set; }

        public ICollection<AnalysisFriendDbModel> AnalysisFriends { get; set; }

        public ICollection<NewSettingsDbModel> NewSettings { get; set; }
    }
}
