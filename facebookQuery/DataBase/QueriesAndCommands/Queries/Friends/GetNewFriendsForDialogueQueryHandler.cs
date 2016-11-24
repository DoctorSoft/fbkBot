using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetNewFriendsForDialogueQueryHandler : IQueryHandler<GetNewFriendsForDialogueQuery, List<FriendData>>
    {
        private readonly DataBaseContext context;

        public GetNewFriendsForDialogueQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendData> Handle(GetNewFriendsForDialogueQuery query)
        {
            var currentTime = DateTime.Now;
            var newFriends = new List<FriendData>();
            try
            {
                var isAvailable =
                    context.Friends.Any(model => model.AccountId == query.AccountId && model.IsBlocked == false);
                if (!isAvailable)
                {
                    return newFriends;
                }

                var friends = context
                    .Friends.Where(
                        model => model.AccountId == query.AccountId && model.IsBlocked == false)
                    .OrderByDescending(model => model.AddedDateTime);

                newFriends.AddRange(from friendDbModel in friends
                    let lastAddedFriendDateTime = friends.FirstOrDefault(model => model.Id == friendDbModel.Id).AddedDateTime
                    let differenceTime = currentTime - lastAddedFriendDateTime
                    where differenceTime.Minutes >= query.DelayTime
                    select new FriendData
                    {
                        Id = friendDbModel.Id, 
                        AccountId = friendDbModel.AccountId, 
                        FacebookId = friendDbModel.FacebookId, 
                        FriendName = friendDbModel.FriendName
                    });
            }
            catch (Exception ex)
            {
                return newFriends;
            }
            return newFriends;
        }
    }
}
