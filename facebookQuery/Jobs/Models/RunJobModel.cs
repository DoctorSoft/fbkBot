using Jobs.Interfaces;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Models
{
    public class RunJobModel : IRunJobModel
    {
        public AccountViewModel Account { get; set; }

        public FriendViewModel Friend { get; set; }

        public bool ForSpy { get; set; }
    }
}
