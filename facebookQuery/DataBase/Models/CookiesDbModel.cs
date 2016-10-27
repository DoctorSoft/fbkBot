namespace DataBase.Models
{
    public class CookiesDbModel
    {
        public long Id { get; set; }

        public AccountDbModel Account { get; set; }

        public string CookiesString { get; set; }
    }
}