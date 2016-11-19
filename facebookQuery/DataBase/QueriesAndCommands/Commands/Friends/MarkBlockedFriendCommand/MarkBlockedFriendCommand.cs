using Constants.FriendsEnums;

namespace DataBase.QueriesAndCommands.Commands.Friends.MarkBlockedFriendCommand
{
    public class MarkBlockedFriendCommand : IVoidCommand
    {
        public long FriendId { get; set; }

        public long AccountId { get; set; }

        public FriendBlockedStatus BlockedCause { get; set; }
    }
}
