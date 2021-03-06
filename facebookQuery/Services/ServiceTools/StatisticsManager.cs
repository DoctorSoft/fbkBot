﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Queries.AccountStatistics;
using DataBase.QueriesAndCommands.Queries.SpyStatistics;
using Services.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class StatisticsManager : IStatisticsManager
    {
        public AccountStatisticsList GetAccountStatistics(long accountId)
        {
            var statisticData = new GetAccountStatisticsQueryHandler(new DataBaseContext()).Handle(new GetAccountStatisticsQuery
            {
                AccountId = accountId
            });

            if (statisticData == null)
            {
                return new AccountStatisticsList
                {
                    StatisticsList = new List<AccountStatisticsModel>()
                };
            }
            return new AccountStatisticsList
            {
                StatisticsList = statisticData.Select(data => new AccountStatisticsModel
                {
                    AccountId = data.AccountId,
                    CreateDateTime = data.CreateDateTime,
                    CountReceivedFriends = data.CountReceivedFriends,
                    Id = data.Id,
                    CountRequestsSentToFriends = data.CountRequestsSentToFriends,
                    CountOrdersConfirmedFriends = data.CountOrdersConfirmedFriends,
                    DateTimeUpdateStatistics = data.DateTimeUpdateStatistics,
                    CountOfWinksBack = data.CountOfWinksBack
                }).ToList()
            };
        }

        public AccountStatisticsModel GetLastHourAccountStatistics(AccountStatisticsList allStatistics)
        {
            return allStatistics.StatisticsList.OrderByDescending(model => model.CreateDateTime).FirstOrDefault();
        }

        public AccountStatisticsModel GetAllTimeAccountStatistics(AccountStatisticsList allStatistics)
        {
            long accountId = 0;
            var updateDateTime = new DateTime();

            var accountStatisticsModel = allStatistics.StatisticsList.OrderByDescending(model => model.DateTimeUpdateStatistics).FirstOrDefault();

            if (accountStatisticsModel == null)
            {
                return new AccountStatisticsModel
                {
                    CountReceivedFriends = allStatistics.StatisticsList.Sum(model => model.CountReceivedFriends),
                    CountRequestsSentToFriends =
                        allStatistics.StatisticsList.Sum(model => model.CountRequestsSentToFriends),
                    CountOrdersConfirmedFriends =
                        allStatistics.StatisticsList.Sum(model => model.CountOrdersConfirmedFriends),
                    CountOfWinksBack = allStatistics.StatisticsList.Sum(model => model.CountOfWinksBack),
                    AccountId = accountId,
                    CreateDateTime = DateTime.Now,
                    DateTimeUpdateStatistics = updateDateTime
                };
            }

            accountId = accountStatisticsModel.AccountId;
            updateDateTime = accountStatisticsModel.DateTimeUpdateStatistics;

            return new AccountStatisticsModel
            {
                CountReceivedFriends = allStatistics.StatisticsList.Sum(model => model.CountReceivedFriends),
                CountRequestsSentToFriends = allStatistics.StatisticsList.Sum(model => model.CountRequestsSentToFriends),
                CountOrdersConfirmedFriends = allStatistics.StatisticsList.Sum(model => model.CountOrdersConfirmedFriends),
                CountOfWinksBack = allStatistics.StatisticsList.Sum(model => model.CountOfWinksBack),
                AccountId = accountId,
                CreateDateTime = DateTime.Now,
                DateTimeUpdateStatistics = updateDateTime
            };
        }

        public void UpdateAccountStatistics(AccountStatisticsModel newStatistics)
        {
            new AddOrUpdateAccountStatisticsCommandHandler(new DataBaseContext()).Handle(
                new AddOrUpdateAccountStatisticsCommand
                {
                    AccountId = newStatistics.AccountId,
                    CountReceivedFriends = newStatistics.CountReceivedFriends,
                    CountRequestsSentToFriends = newStatistics.CountRequestsSentToFriends,
                    CreateDateTime = newStatistics.CreateDateTime,
                    CountOfWinksBack = newStatistics.CountOfWinksBack,
                    DateTimeUpdateStatistics = newStatistics.DateTimeUpdateStatistics
                });
        }

        public SpyStatisticsList GetSpyStatistics(long spyId)
        {
            var statisticData = new GetSpyStatisticsQueryHandler(new DataBaseContext()).Handle(new GetSpyStatisticsQuery
            {
                SpyId = spyId
            });

            if (statisticData == null)
            {
                return new SpyStatisticsList
                {
                    StatisticsList = new List<SpyStatisticsModel>()
                };
            }
            return new SpyStatisticsList
            {
                StatisticsList = statisticData.Select(data => new SpyStatisticsModel
                {
                    SpyAccountId = data.SpyAccountId,
                    CreateDateTime = data.CreateDateTime,
                    CountAnalizeFriends = data.CountAnalizeFriends,
                    Id = data.Id,
                    DateTimeUpdateStatistics = data.DateTimeUpdateStatistics
                }).ToList()
            };
        }

        public SpyStatisticsModel GetLastHourSpyStatistics(SpyStatisticsList allStatistics)
        {
            return allStatistics.StatisticsList.OrderByDescending(model => model.CreateDateTime).FirstOrDefault();
        }

        public SpyStatisticsModel GetAllTimeSpyStatistics(SpyStatisticsList allStatistics)
        {
            long accountId = 0;
            var updateDateTime = new DateTime();

            var accountStatisticsModel = allStatistics.StatisticsList.OrderByDescending(model => model.DateTimeUpdateStatistics).FirstOrDefault();

            if (accountStatisticsModel == null)
                return new SpyStatisticsModel
                {
                    CountAnalizeFriends = allStatistics.StatisticsList.Sum(model => model.CountAnalizeFriends),
                    SpyAccountId = accountId,
                    CreateDateTime = DateTime.Now,
                    DateTimeUpdateStatistics = updateDateTime
                };

            accountId = accountStatisticsModel.SpyAccountId;
            updateDateTime = accountStatisticsModel.DateTimeUpdateStatistics;
            return new SpyStatisticsModel
            {
                CountAnalizeFriends = allStatistics.StatisticsList.Sum(model => model.CountAnalizeFriends),
                SpyAccountId = accountId,
                CreateDateTime = DateTime.Now,
                DateTimeUpdateStatistics = updateDateTime
            };
        }
    }
}
