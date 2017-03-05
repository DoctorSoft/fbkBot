using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.SpyStatistics
{
    public class GetSpyStatisticsQueryHandler : IQueryHandler<GetSpyStatisticsQuery, List<SpyStatisticsData>>
    {
        private readonly DataBaseContext context;

        public GetSpyStatisticsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<SpyStatisticsData> Handle(GetSpyStatisticsQuery query)
        {
            try
            {
                var statistics =
                    context.SpyStatistics.Where(model => model.SpyId == query.SpyId)
                        .Select(model => new SpyStatisticsData
                        {
                            SpyAccountId = model.Id,
                            CreateDateTime = model.CreateDateTime,
                            Id = model.Id,
                            CountAnalizeFriends = model.CountAnalizeFriends,
                            DateTimeUpdateStatistics = model.DateTimeUpdateStatistics
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
