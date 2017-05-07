using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class GetFriendsToConfirmQueryHandler : IQueryHandler<GetFriendsToConfirmQuery, List<AnalysisFriendsData>>
    {
        private readonly DataBaseContext context;

        public GetFriendsToConfirmQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AnalysisFriendsData> Handle(GetFriendsToConfirmQuery query)
        {
            var friendsData = context
                .AnalisysFriends
                .Where(model => model.AccountId == query.AccountId 
                    //&& model.Status == StatusesFriend.ToAdd  // без гео
                    && model.Type == FriendTypes.Incoming)
                .Select(model => new AnalysisFriendsData
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    FacebookId = model.FacebookId,
                    FriendName = model.FriendName,
                    Type = model.Type,
                    AddedDateTime = model.AddedDateTime,
                    Status = model.Status
                }).ToList();

            return friendsData;
        }
    }
}
