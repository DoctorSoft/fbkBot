namespace Services.ViewModels.AccountInformationModels
{
    public class AccountInformationViewModel
    {
        public long Id { get; set; }        
        
        public long CountCurrentFriends { get; set; }

        public long CountIncommingFriendsRequest { get; set; }

        public int CountNewMessages { get; set; }
    }
}
