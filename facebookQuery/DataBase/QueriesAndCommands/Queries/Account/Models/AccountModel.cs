namespace DataBase.QueriesAndCommands.Queries.Account.Models
{
    public class AccountModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }
        
        public string Name { get; set; }

        public long FacebookId { get; set; }

        public string Proxy { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }

        public long? GroupSettingsId { get; set; }

        public bool ProxyDataIsFailed { get; set; }

        public bool AuthorizationDataIsFailed { get; set; }

        public bool ConformationIsFailed { get; set; }

        public bool IsDeleted { get; set; }

        public long? UserAgentId { get; set; }

        public CookieModel Cookie { get; set; }
    }
}
