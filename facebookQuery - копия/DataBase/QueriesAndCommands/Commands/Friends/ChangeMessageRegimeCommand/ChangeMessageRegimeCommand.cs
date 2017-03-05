using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Commands.Friends.ChangeMessageRegimeCommand
{
    public class ChangeMessageRegimeCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }

        public MessageRegime MessageRegime { get; set; }
    }
}
