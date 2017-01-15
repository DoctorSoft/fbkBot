using CommonModels;
using Services.ViewModels.FriendsModels;

namespace Services.Core.Interfaces
{
    public interface IAnalyzeFriendCore
    {
        void StartAnalyze(SettingsModel settings, FriendInfoViewModel friendInfo);
    }
}
