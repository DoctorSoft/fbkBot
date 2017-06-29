using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Constants.MessageEnums;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsByMessageRegime
{
    public class GetFriendsByMessageRegimeQueryHandler : IQueryHandler<GetFriendsByMessageRegimeQuery,
        FriendListForPaging>
    {
        private readonly DataBaseContext _context;

        public GetFriendsByMessageRegimeQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FriendListForPaging Handle(GetFriendsByMessageRegimeQuery query)
        {
            try
            {
                var pageInfo = query.Page;
                var regime = query.MessageRegime == MessageRegime.NoMessages ? null : query.MessageRegime;

                IQueryable<FriendDbModel> friends;
                var countFriends = _context.Friends.Include(friend => friend.FriendMessages).Count(
                    model => model.AccountId == query.AccountId
                             && !_context.FriendsBlackList.Any(
                                 dbModel => dbModel.FriendFacebookId == model.FacebookId &&
                                            dbModel.GroupId == query.GroupSettingsId)
                             && model.MessageRegime == query.MessageRegime);

                var result = new FriendListForPaging
                {
                    Friends = new List<FriendData>(),
                    CountAllFriends = countFriends
                };

                if (pageInfo != null && pageInfo.PageNumber != 0 && pageInfo.PageSize != 0)
                {
                    var pageNumber = pageInfo.PageNumber;
                    var pageSize = pageInfo.PageSize;

                    friends = _context.Friends.Include(friend => friend.FriendMessages).Where(
                            model => model.AccountId == query.AccountId
                                     && !_context.FriendsBlackList.Any(
                                         dbModel => dbModel.FriendFacebookId == model.FacebookId &&
                                                    dbModel.GroupId == query.GroupSettingsId)
                                     && model.MessageRegime == regime)
                        .OrderByDescending(model => model.DeleteFromFriends)
                        .Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    friends = _context.Friends.Include(friend => friend.FriendMessages).Where(
                        model => model.AccountId == query.AccountId
                                 && !_context.FriendsBlackList.Any(
                                     dbModel => dbModel.FriendFacebookId == model.FacebookId &&
                                                dbModel.GroupId == query.GroupSettingsId)
                                 && model.MessageRegime == regime);
                }

                var friendsListResponse = friends.Select(friendDbModel => new FriendData
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
                }).ToList();

                result.Friends = friendsListResponse;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
