using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQueryHandler : IQueryHandler<GetFriendsByAccountQuery, FriendListForPaging>
    {
        private readonly DataBaseContext _context;

        public GetFriendsByAccountQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FriendListForPaging Handle(GetFriendsByAccountQuery query)
        {
            var pageInfo = query.Page;

            IQueryable<FriendDbModel> friends;
            var countFriends = _context.Friends.Count(model => model.AccountId == query.AccountId);
            var result = new FriendListForPaging
            {
                Friends = new List<FriendData>(),
                CountAllFriends = countFriends
            };

            if (pageInfo != null && pageInfo.PageNumber != 0 && pageInfo.PageSize != 0)
            {
                var pageNumber = pageInfo.PageNumber;
                var pageSize = pageInfo.PageSize;
                friends = _context.Friends.Where(model => model.AccountId == query.AccountId)
                    .OrderByDescending(model => model.DeleteFromFriends)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                friends = _context.Friends.Where(model => model.AccountId == query.AccountId);
            }

            var friendsResponseModels = friends.Select(model => new FriendData
            {
                FacebookId = model.FacebookId,
                AccountId = model.AccountId,
                FriendName = model.FriendName,
                Deleted = model.DeleteFromFriends,
                Id = model.Id,
                DialogIsCompleted = model.DialogIsCompleted,
                MessageRegime = model.MessageRegime,
                AddedDateTime = model.AddedDateTime,
                Href = model.Href,
                IsAddedToGroups = model.IsAddedToGroups,
                IsAddedToPages = model.IsAddedToPages,
                IsWinked = model.IsWinked,
                Gender = model.Gender,
                AddedToRemoveDateTime = model.AddedToRemoveDateTime,
                IsWinkedFriendsFriend = model.IsWinkedFriendsFriend,
                CountWinksToFriends = model.CountWinksToFriends
            }).ToList();

            result.Friends = friendsResponseModels;

            return result;
        }
    }
}
