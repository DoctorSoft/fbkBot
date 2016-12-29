using DataBase.QueriesAndCommands.Queries.Account.Models;
using Services.ViewModels.FriendsModels;

namespace Services.Core.Interfaces
{
    public interface IAnalyzeFriendCore
    {
        void StartAnalyze(AccountSettingsModel settings, FriendInfoViewModel friendInfo);
    }
}
