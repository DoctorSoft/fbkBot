﻿using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQueryHandler : IQueryHandler<GetFriendByIdAccountQuery, FriendData>
    {
        private readonly DataBaseContext context;

        public GetFriendByIdAccountQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public FriendData Handle(GetFriendByIdAccountQuery query)
        {
            var result = context.Friends
                .Where(model => model.Id == query.AccountId)
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    MessagesEnded = context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId 
                        && dbModel.GroupId == context.Accounts.FirstOrDefault(accountDbModel => accountDbModel.Id == query.AccountId).GroupSettingsId),
                    MessageRegime = model.MessageRegime
                }).FirstOrDefault();

            return result;
        }
    }
}
