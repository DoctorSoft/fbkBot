using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.AnalysisFriends
{
    public class GetFriendsByAccountIdQueryHandler : IQueryHandler<GetFriendsByAccountIdQuery, List<AnalysisFriendsData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsByAccountIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<AnalysisFriendsData> Handle(GetFriendsByAccountIdQuery query)
        {
            List<AnalysisFriendsData> friendsData;
            if (query.FriendsType == null)
            {
                friendsData = _context
                .AnalisysFriends
                .Where(model => model.AccountId == query.AccountId
                    && model.Status == query.Status)
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
            if (query.Status == null)
            {
                friendsData = _context
                    .AnalisysFriends
                    .Where(model => model.AccountId == query.AccountId
                                    && model.Type == query.FriendsType)
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

            friendsData = _context
            .AnalisysFriends
            .Where(model => model.AccountId == query.AccountId
                            && model.Type == query.FriendsType
                            && model.Status == query.Status)
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
