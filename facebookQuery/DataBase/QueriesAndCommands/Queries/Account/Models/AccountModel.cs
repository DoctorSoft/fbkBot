namespace DataBase.QueriesAndCommands.Queries.Account.Models
{
    public class AccountModel
    {
        public long Id { get; set; }

        public string PageUrl { get; set; }

        public CookieModel Cookie { get; set; }
    }
}
