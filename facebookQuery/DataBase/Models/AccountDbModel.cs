namespace DataBase.Models
{
    public class AccountDbModel
    {
        public long Id { get; set; }

        public string PageUrl { get; set; }

        public CookiesDbModel Cookies { get; set; }
    }
}
