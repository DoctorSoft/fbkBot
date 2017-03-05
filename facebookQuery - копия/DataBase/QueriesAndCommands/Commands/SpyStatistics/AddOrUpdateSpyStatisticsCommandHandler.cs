using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.SpyStatistics
{
    public class AddOrUpdateSpyStatisticsCommandHandler : ICommandHandler<AddOrUpdateSpyStatisticsCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateSpyStatisticsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateSpyStatisticsCommand command)
        {
            var spyAccountStatistics = context.SpyStatistics.OrderByDescending(model=>model.CreateDateTime).FirstOrDefault(model => model.SpyId == command.SpyAccountId);

            if (spyAccountStatistics == null)
            {
                spyAccountStatistics = new SpyStatisticsDbModel
                {
                    CountAnalizeFriends = command.CountAnalizeFriends,
                    SpyId = command.SpyAccountId,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                context.SpyStatistics.AddOrUpdate(spyAccountStatistics);

                context.SaveChanges();

                return spyAccountStatistics.Id;
            }

            if (CheckDelay(spyAccountStatistics.CreateDateTime))
            {
                var newSpyStatisticsStatistics = new SpyStatisticsDbModel
                {
                    CountAnalizeFriends = command.SpyAccountId,
                    SpyId = command.SpyAccountId,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                context.SpyStatistics.AddOrUpdate(newSpyStatisticsStatistics);

                context.SaveChanges();

                return spyAccountStatistics.Id;
            }

            spyAccountStatistics.SpyId = command.SpyAccountId;
            if (command.CountAnalizeFriends != null)
            {
                spyAccountStatistics.CountAnalizeFriends = command.CountAnalizeFriends + spyAccountStatistics.CountAnalizeFriends;
            }

            spyAccountStatistics.DateTimeUpdateStatistics = DateTime.Now;

            context.SpyStatistics.AddOrUpdate(spyAccountStatistics);

            context.SaveChanges();

            return spyAccountStatistics.Id;
        }

        private bool CheckDelay(DateTime updateStatisticsDateTime)
        {
            var differenceTime = DateTime.Now - updateStatisticsDateTime;
            var summ = differenceTime.Days * 24 * 60 + differenceTime.Hours * 60 + differenceTime.Minutes;
            return summ >= 60; //1 Hour
        }
    }
}
