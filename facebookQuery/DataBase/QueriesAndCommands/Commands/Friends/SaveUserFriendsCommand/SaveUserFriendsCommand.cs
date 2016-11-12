﻿using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand
{
    public class SaveUserFriendsCommand : IVoidCommand
    {
        public long AccountId { get; set; }

        public List<GetFriendsResponseModel> Friends { get; set; }
    }
}
