using DataBase.QueriesAndCommands.Queries.Friends;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveNewFriendCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public FriendData FriendData { get; set; }
    }
}
