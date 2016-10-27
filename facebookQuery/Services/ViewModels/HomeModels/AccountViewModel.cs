namespace Services.ViewModels.HomeModels
{
    public class AccountViewModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
        
        public long UserId { get; set; }

        public string PageUrl { get; set; }
    }
}