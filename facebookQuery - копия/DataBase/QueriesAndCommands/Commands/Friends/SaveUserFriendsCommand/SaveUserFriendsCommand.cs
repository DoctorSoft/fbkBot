using System.Collections.Generic;
using CommonModels;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Friends;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveUserFriendsCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<FriendData> Friends { get; set; }
    }
}
