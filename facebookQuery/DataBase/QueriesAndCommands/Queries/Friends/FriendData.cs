using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class FriendData
    {
        public long Id { get; set; }
        
        public long FacebookId { get; set; }

        public long AccountId { get; set; }

        public string FriendName { get; set; }

        public bool Deleted { get; set; }

        public bool MessagesEnded { get; set; }

        public MessageRegime? MessageRegime { get; set; }
    }
}
