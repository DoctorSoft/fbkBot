using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsForAddedToGroup
{
    public class GetFriendsForAddedToGroupQueryHandler : IQueryHandler<GetFriendsForAddedToGroupQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsForAddedToGroupQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetFriendsForAddedToGroupQuery query)
        {
            try
            {
                var friendsForAddedToGroup = _context.Friends
                    .Where(model => model.AccountId == query.AccountId) // друзья аккаунта
                    .Where(model => !model.IsAddedToGroups) // не в группе
                    .Where(model => !model.DeleteFromFriends) // не удалился из друзей
                    //.Where(model => model.DialogIsCompleted) // завершен диалог
                    .Where(model => !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId)) // не в черном списке
                    .Take(query.Count).ToList(); // берем N друзей

                return friendsForAddedToGroup.Select(model => new FriendData
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    FacebookId = model.FacebookId,
                    Gender = model.Gender,
                    FriendName = model.FriendName,
                    Href = model.Href,
                    Deleted = model.DeleteFromFriends,
                    MessageRegime = model.MessageRegime,
                    DialogIsCompleted = model.DialogIsCompleted
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
