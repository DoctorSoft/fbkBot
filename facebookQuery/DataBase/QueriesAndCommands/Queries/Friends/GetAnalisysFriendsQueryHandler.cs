using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetAnalisysFriendsQueryHandler : IQueryHandler<GetAnalisysFriendsQuery, List<AnalysisFriendData>>
    {
        private readonly DataBaseContext context;

        public GetAnalisysFriendsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AnalysisFriendData> Handle(GetAnalisysFriendsQuery query)
        {
            try
            {
                var result = context.AnalisysFriends
                .Where(model => model.Status == StatusesFriend.ToAnalys)
                .GroupBy(model => model.AccountId)
                .Select(model => model.OrderBy(dbModel => dbModel.AddedDateTime).FirstOrDefault()).ToList();

                return result.Select(model => new AnalysisFriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Id = model.Id,
                    AddedToAnalysDateTime = model.AddedDateTime,
                    Type = model.Type,
                    Status = model.Status
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
