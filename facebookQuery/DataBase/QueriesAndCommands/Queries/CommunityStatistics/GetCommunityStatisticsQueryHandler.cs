using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.AccountStatistics;

namespace DataBase.QueriesAndCommands.Queries.CommunityStatistics
{
    public class GetCommunityStatisticsQueryHandler : IQueryHandler<GetCommunityStatisticsQuery, List<CommunityStatisticsData>>
    {
        private readonly DataBaseContext _context;

        public GetCommunityStatisticsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<CommunityStatisticsData> Handle(GetCommunityStatisticsQuery query)
        {
            try
            {
                var statistics =
                    _context.CommunityStatistics.Where(model => model.GroupId == query.GroupId)
                    .Where(model => model.UpdateDateTime == DateTime.Today)
                    .Select(model => new CommunityStatisticsData
                    {
                        AccountId = model.Id,
                        Id = model.Id,
                        CountOfGroupInvitations = model.CountOfGroupInvitations,
                        CountOfPageInvitations = model.CountOfGroupInvitations,
                        UpdateDateTime = model.UpdateDateTime
                    }).ToList();

                return statistics;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
