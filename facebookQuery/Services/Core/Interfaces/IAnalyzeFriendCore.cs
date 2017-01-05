using CommonModels;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces
{
    public interface IAnalyzeFriendCore
    {
        void StartAnalyze(AccountSettingsModel settings, FriendInfoViewModel friendInfo);
    }
}
