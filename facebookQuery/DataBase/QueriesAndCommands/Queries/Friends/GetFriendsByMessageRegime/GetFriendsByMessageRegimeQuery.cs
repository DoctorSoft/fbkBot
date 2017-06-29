using System.Collections.Generic;
using Constants.MessageEnums;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsByMessageRegime
{
    public class GetFriendsByMessageRegimeQuery : IQuery<FriendListForPaging>
    {
        public long AccountId { get; set; }

        public MessageRegime? MessageRegime { get; set; }

        public long? GroupSettingsId { get; set; }

        public PageModel Page { get; set; }
    }
}
