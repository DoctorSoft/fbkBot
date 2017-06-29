using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Interfaces
{
    public interface IRunJobModel
    {
        AccountViewModel Account { get; set; }

        FriendViewModel Friend { get; set; }

        bool ForSpy { get; set; }
    }
}
