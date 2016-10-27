namespace DataBase.QueriesAndCommands.Queries.Account.Models
{
    public class AccountModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long UserId { get; set; }

        public CookieModel Cookie { get; set; }
    }
}
