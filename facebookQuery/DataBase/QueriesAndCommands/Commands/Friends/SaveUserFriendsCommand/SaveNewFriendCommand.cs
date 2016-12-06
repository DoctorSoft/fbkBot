using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveNewFriendCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public FriendDbModel FriendData { get; set; }
    }
}
