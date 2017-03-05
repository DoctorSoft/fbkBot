using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetAnalisysFriendsByStatus
{
    public class GetAnalisysFriendsByStatusQueryHandler : IQueryHandler<GetAnalisysFriendsByStatusQuery, List<AnalysisFriendData>>
    {
        private readonly DataBaseContext context;

        public GetAnalisysFriendsByStatusQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<AnalysisFriendData> Handle(GetAnalisysFriendsByStatusQuery query)
        {
            try
            {
                var analisisFriends = context.AnalisysFriends
                .Where(model => model.Status == query.Status)
                .Select(model => new AnalysisFriendData
                    {
                        FacebookId = model.FacebookId,
                        AccountId = model.AccountId,
                        FriendName = model.FriendName,
                        Id = model.Id,
                        AddedToAnalysDateTime = model.AddedDateTime,
                        Type = model.Type,
                        Status = model.Status
                    }).ToList();


                return analisisFriends;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
