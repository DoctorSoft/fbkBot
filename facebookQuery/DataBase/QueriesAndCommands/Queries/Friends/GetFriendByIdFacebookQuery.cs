using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdFacebookQuery : IQuery<FriendData>
    {
        public long FacebookId { get; set; }
    }
}
