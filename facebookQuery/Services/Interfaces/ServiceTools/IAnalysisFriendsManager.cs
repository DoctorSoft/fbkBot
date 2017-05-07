using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;
using Services.Interfaces.Notices;
using Services.ViewModels.HomeModels;

namespace Services.Interfaces.ServiceTools
{
    public interface IAnalysisFriendsManager
    {
        List<AnalysisFriendData> CheckForAnyInDataBase(AccountViewModel account, List<AnalysisFriendData> friends, INoticesProxy notices, string functionName);
    }
}
