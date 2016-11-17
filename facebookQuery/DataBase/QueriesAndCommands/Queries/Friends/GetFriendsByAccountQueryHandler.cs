﻿using System.Collections.Generic;
using System.Linq;
using CommonModels;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQueryHandler : IQueryHandler<GetFriendsByAccountQuery, List<FriendData>>
    {
        private readonly DataBaseContext context;

        public GetFriendsByAccountQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendData> Handle(GetFriendsByAccountQuery query)
        {
            var result = context.Friends
                .Where(model => model.AccountId == query.AccountId)
                .Select(model => new FriendData
                {
                    FriendId = model.FriendId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id
                }).ToList();

            return result;
        }
    }
}