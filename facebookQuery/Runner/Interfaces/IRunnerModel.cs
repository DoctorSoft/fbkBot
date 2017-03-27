using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Runner.Interfaces
{
    public interface IRunnerModel
    {
        AccountViewModel Account { get; set; }

        FriendViewModel Friend { get; set; }
    }
}
