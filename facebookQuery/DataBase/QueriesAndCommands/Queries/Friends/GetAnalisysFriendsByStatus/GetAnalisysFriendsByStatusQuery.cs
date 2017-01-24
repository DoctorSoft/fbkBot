using System.Collections.Generic;
using Constants.FriendTypesEnum;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetAnalisysFriendsByStatus
{
    public class GetAnalisysFriendsByStatusQuery : IQuery<List<AnalysisFriendData>>
    {
        public StatusesFriend Status { get; set; }
    }
}
