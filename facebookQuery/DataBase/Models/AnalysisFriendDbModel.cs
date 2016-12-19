using System;
using Constants.FriendTypesEnum;

namespace DataBase.Models
{
    public class AnalysisFriendDbModel
    {
        public long Id { get; set; }

        public long FacebookId { get; set; }

        public string FriendName { get; set; }
        
        public long AccountId { get; set; }
        
        public AccountDbModel AccountWithFriend { get; set; }
        
        public DateTime AddedDateTime { get; set; }

        public StatusesFriend Status { get; set; }
    }
}
