using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetAllFriendByQueryHandler : IQueryHandler<GetAllFriendByQuery, List<FriendData>>
    {
        private readonly DataBaseContext context;

        public GetAllFriendByQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendData> Handle(GetAllFriendByQuery query)
        {
            var result = context.Friends
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    MessagesEnded = context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId
                        && dbModel.GroupId == context.Accounts.FirstOrDefault(accountDbModel => accountDbModel.Id == model.AccountId).GroupSettingsId),
                    MessageRegime = model.MessageRegime
                }).ToList();

            return result;
        }
    }
}
