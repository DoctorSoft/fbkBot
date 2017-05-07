using CommonInterfaces.Interfaces.Models;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Services.Models.BackgroundJobs
{
    public class AddOrUpdateAccountModel : IAddOrUpdateAccountJobs
    {
        public AccountViewModel Account { get; set; }

        public GroupSettingsViewModel NewSettings { get; set; }

        public GroupSettingsViewModel OldSettings { get; set; }

        public FriendViewModel Friend { get; set; }

        public bool IsForSpy { get; set; }
    }
}
