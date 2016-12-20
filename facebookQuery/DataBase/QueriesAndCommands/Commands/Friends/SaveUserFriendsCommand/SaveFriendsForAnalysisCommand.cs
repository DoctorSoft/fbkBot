using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveFriendsForAnalysisCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<AnalysisFriendData> Friends { get; set; }
    }
}
