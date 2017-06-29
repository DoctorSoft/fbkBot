using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            try { 
                var friends =
                    _context.Friends.Include(friend => friend.FriendMessages).Where(model => model.AccountId == query.AccountId
                        && !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId && dbModel.GroupId == query.GroupSettingsId) 
                        && !model.DeleteFromFriends 
                        && model.FriendMessages.All(message => message.FriendId != model.Id))
                        .Take(query.CountFriend).ToList();

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
                            FacebookId = friendDbModel.FacebookId,
                            AccountId = friendDbModel.AccountId,
                            FriendName = friendDbModel.FriendName,
                            Deleted = friendDbModel.DeleteFromFriends,
                            Id = friendDbModel.Id,
                            DialogIsCompleted = friendDbModel.DialogIsCompleted,
                            MessageRegime = friendDbModel.MessageRegime,
                            AddedDateTime = friendDbModel.AddedDateTime,
                            Href = friendDbModel.Href,
                            IsAddedToGroups = friendDbModel.IsAddedToGroups,
                            IsAddedToPages = friendDbModel.IsAddedToPages,
                            IsWinked = friendDbModel.IsWinked,
                            Gender = friendDbModel.Gender,
                            AddedToRemoveDateTime = friendDbModel.AddedToRemoveDateTime,
                            IsWinkedFriendsFriend = friendDbModel.IsWinkedFriendsFriend
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
