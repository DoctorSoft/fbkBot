using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetNewFriendsForDialogueQueryHandler : IQueryHandler<GetNewFriendsForDialogueQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetNewFriendsForDialogueQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetNewFriendsForDialogueQuery query)
        {
            var newFriends = new List<FriendData>();
            try
            {
                var friends =
                    _context.Friends.Where(model => model.AccountId == query.AccountId
                        && !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId && dbModel.GroupId == query.AccountId) 
                        && !model.DeleteFromFriends 
                        && !model.FriendMessages.Any()).ToList();

                var allMessages = _context.FriendMessages.Select(model => model);

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
                    if (CheckDelay(friendDbModel.AddedDateTime, query.DelayTime))
                    {
                        newFriends.Add(new FriendData
                        {
                            AccountId = friendDbModel.AccountId,
                            FacebookId = friendDbModel.FacebookId,
                            FriendName = friendDbModel.FriendName,
                            Id = friendDbModel.Id
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return newFriends;
            }
            return newFriends;
        }

        private bool CheckDelay(DateTime friendAddedDateTime, int delay)
        {
            var s = DateTime.Now - friendAddedDateTime;
            var summ = s.Days * 24 * 60 + s.Hours * 60 + s.Minutes;
            return summ > delay;
        }
    }
}
