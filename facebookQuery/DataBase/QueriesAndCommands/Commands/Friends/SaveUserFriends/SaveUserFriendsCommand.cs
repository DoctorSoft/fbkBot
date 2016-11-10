using System.Collections.Generic;
using Engines.Engines.GetFriendsEngine;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriends
{
    public class SaveUserFriendsCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<GetFriendsResponseModel> Friends { get; set; }
    }
}
