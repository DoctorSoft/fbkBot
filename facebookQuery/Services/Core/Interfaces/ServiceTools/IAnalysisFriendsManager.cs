﻿using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAnalysisFriendsManager
    {
        List<AnalysisFriendData> CheckForAnyInDataBase(AccountModel account, List<AnalysisFriendData> friends);
    }
}
