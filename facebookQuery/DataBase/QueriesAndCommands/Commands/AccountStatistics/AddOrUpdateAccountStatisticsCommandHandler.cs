using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.AccountStatistics
{
    public class AddOrUpdateAccountStatisticsCommandHandler : ICommandHandler<AddOrUpdateAccountStatisticsCommand, long>
    {
        private readonly DataBaseContext _context;

        public AddOrUpdateAccountStatisticsCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public long Handle(AddOrUpdateAccountStatisticsCommand command)
        {
            var accountStatistics = _context.AccountStatistics.OrderByDescending(model=>model.CreateDateTime).FirstOrDefault(model => model.AccountId == command.AccountId);

            if (accountStatistics == null)
            {
                accountStatistics = new AccountStatisticsDbModel
                {
                    AccountId = command.AccountId,
                    CountReceivedFriends = command.CountReceivedFriends,
                    CountRequestsSentToFriends = command.CountRequestsSentToFriends,
                    CountOrdersConfirmedFriends = command.CountOrdersConfirmedFriends,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                _context.AccountStatistics.AddOrUpdate(accountStatistics);

                _context.SaveChanges();

                return accountStatistics.Id;
            }

            if (CheckDelay(accountStatistics.CreateDateTime))
            {
                var newAccountStatistics = new AccountStatisticsDbModel
                {
                    AccountId = command.AccountId,
                    CountReceivedFriends = command.CountReceivedFriends,
                    CountRequestsSentToFriends = command.CountRequestsSentToFriends,
                    CountOrdersConfirmedFriends = command.CountOrdersConfirmedFriends,
                    DateTimeUpdateStatistics = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                _context.AccountStatistics.AddOrUpdate(newAccountStatistics);

                _context.SaveChanges();

                return accountStatistics.Id;
            }

            accountStatistics.AccountId = command.AccountId;

            if (command.CountReceivedFriends != 0)
            {
                accountStatistics.CountReceivedFriends = command.CountReceivedFriends + accountStatistics.CountReceivedFriends;
            }
            if (command.CountRequestsSentToFriends != 0)
            {
                accountStatistics.CountRequestsSentToFriends = command.CountRequestsSentToFriends + accountStatistics.CountRequestsSentToFriends;
            }
            if (command.CountOrdersConfirmedFriends != 0)
            {
                accountStatistics.CountOrdersConfirmedFriends = command.CountOrdersConfirmedFriends + accountStatistics.CountOrdersConfirmedFriends;
            }
            if (command.CountOfWinksBack != 0)
            {
                accountStatistics.CountOrdersConfirmedFriends = command.CountOfWinksBack + accountStatistics.CountOfWinksBack;
            }

            accountStatistics.DateTimeUpdateStatistics = DateTime.Now;

            _context.AccountStatistics.AddOrUpdate(accountStatistics);

            _context.SaveChanges();

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
