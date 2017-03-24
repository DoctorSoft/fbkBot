using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.ScheduleDeleteFriends
{
    public class GetScheduleDeleteFriendsQueryHandler : IQueryHandler<GetScheduleDeleteFriendsQuery, List<ScheduleRemovalOfFriendsModel>>
    {
        private readonly DataBaseContext _context;

        public GetScheduleDeleteFriendsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<ScheduleRemovalOfFriendsModel> Handle(GetScheduleDeleteFriendsQuery command)
        {
            var result = _context.ScheduleRemovalOfFriends.Select(model => new ScheduleRemovalOfFriendsModel
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                FriendId = model.FriendId,
                AddDateTime = model.AddDateTime,
                Id = model.Id
            }).ToList();

            return result;
        }
    }
}
