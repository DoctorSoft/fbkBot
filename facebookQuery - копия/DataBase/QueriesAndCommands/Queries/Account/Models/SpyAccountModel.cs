namespace DataBase.QueriesAndCommands.Queries.Account.Models
{
    public class SpyAccountModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long UserId { get; set; }

        public string Name { get; set; }

        public long FacebookId { get; set; }

        public string Proxy { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }

        public CookieModel Cookie { get; set; }
    }
}
