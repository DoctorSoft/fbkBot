using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendById
{
    public class GetFriendByIdQuery : IQuery<FriendData>
    {
        public long FriendId { get; set; }
    }
}
