using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.AccountStatistics
{
    public class AddOrUpdateAccountStatisticsCommandHandler : ICommandHandler<AddOrUpdateAccountStatisticsCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateAccountStatisticsCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateAccountStatisticsCommand command)
        {
            var accountStatistics = context.AccountStatistics.OrderByDescending(model=>model.CreateDateTime).FirstOrDefault(model => model.AccountId == command.AccountId);

            if (accountStatistics == null)
            {
                accountStatistics = new AccountStatisticsDbModel
                {
                    AccountId = command.AccountId,
                    CountReceivedFriends = command.CountReceivedFriends,
                    CountRequestsSentToFriends = command.CountRequestsSentToFriends,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                context.AccountStatistics.AddOrUpdate(accountStatistics);

                context.SaveChanges();

                return accountStatistics.Id;
            }

            if (CheckDelay(accountStatistics.CreateDateTime))
            {
                var newAccountStatistics = new AccountStatisticsDbModel
                {
                    AccountId = command.AccountId,
                    CountReceivedFriends = command.CountReceivedFriends,
                    CountRequestsSentToFriends = command.CountRequestsSentToFriends,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                context.AccountStatistics.AddOrUpdate(newAccountStatistics);

                context.SaveChanges();

                return accountStatistics.Id;
            }

            accountStatistics.AccountId = command.AccountId;
            if (command.CountReceivedFriends != null)
            {
                accountStatistics.CountReceivedFriends = command.CountReceivedFriends + accountStatistics.CountReceivedFriends;
            }
            if (command.CountRequestsSentToFriends != null)
            {
                accountStatistics.CountRequestsSentToFriends = command.CountRequestsSentToFriends + accountStatistics.CountRequestsSentToFriends;
            }

            accountStatistics.DateTimeUpdateStatistics = DateTime.Now;

            context.AccountStatistics.AddOrUpdate(accountStatistics);

            context.SaveChanges();

            return accountStatistics.Id;
        }

        private bool CheckDelay(DateTime updateStatisticsDateTime)
        {
            var differenceTime = DateTime.Now - updateStatisticsDateTime;
            var summ = differenceTime.Days * 24 * 60 + differenceTime.Hours * 60 + differenceTime.Minutes;
            return summ >= 60; //1 Hour
        }
    }
}
