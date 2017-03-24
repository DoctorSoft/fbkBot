using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.DeleteSchedule
{
    public class GetFriendsToQueueDeletionQueryHandler : IQueryHandler<GetFriendsToQueueDeletionQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsToQueueDeletionQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetFriendsToQueueDeletionQuery query)
        {
            try
            {
                var analisisFriends = _context.Friends
                .Where(model => !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId)); //не в чс

                var analisisFriendsNotScheduleToRemove = analisisFriends.Where(model => (!_context.ScheduleRemovalOfFriends.Any(data => data.FriendId == model.Id && data.AccountId == query.AccountId)))
                .Select(model => new FriendData
                    {
                        Id = model.Id,
                        FacebookId = model.FacebookId,
                        AccountId = model.AccountId,
                        FriendName = model.FriendName,
                        Deleted = model.DeleteFromFriends,
                        AddedDateTime = model.AddedDateTime
                    }).ToList();


                return analisisFriendsNotScheduleToRemove;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
