namespace Services.ViewModels.HomeModels
{
    public class AccountActionModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public long UserId { get; set; }

        public string PageUrl { get; set; }

        public int NumberNewFriends { get; set; }

        public int NumberNewMessages { get; set; }

        public int NumberNewNotifications { get; set; }
    }
}
