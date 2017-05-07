using Runner.Interfaces;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Runner.Models
{
    public class RunnerModel : IRunnerModel
    {
        public AccountViewModel Account { get; set; }

        public FriendViewModel Friend { get; set; }
        public bool ForSpy { get; set; }
    }
}
