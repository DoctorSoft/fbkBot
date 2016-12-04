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
            var newFriends = new List<FriendData>();
            try
            {
                var friends =
                    context.Friends.Where(model => model.AccountId == query.AccountId && model.IsBlocked == false).ToList();
                var allMessages =context.FriendMessages.Select(model => model);

                if (!friends.Any())
                {
                    return newFriends;
                }

                foreach (var friendDbModel in friends)
                {
                    var messagesFound =
                        allMessages.Any(model => model.FriendId == friendDbModel.Id);

                    if (messagesFound)
                    {
                        continue;
                    }
                    if (friendDbModel.AddedDateTime != null)
                    {
                        if ((DateTime.Now - friendDbModel.AddedDateTime).Minutes > query.DelayTime)
                        {
                            newFriends.Add(new FriendData()
                            {
                                AccountId = friendDbModel.AccountId,
                                FacebookId = friendDbModel.FacebookId,
                                FriendName = friendDbModel.FriendName,
                                Id = friendDbModel.Id
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return newFriends;
            }
            return newFriends;
        }
    }
}
