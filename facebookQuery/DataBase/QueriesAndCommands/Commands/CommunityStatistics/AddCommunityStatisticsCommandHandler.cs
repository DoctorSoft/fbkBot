using System;
using System.Data.Entity.Migrations;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.CommunityStatistics
{
    public class AddCommunityStatisticsCommandHandler : ICommandHandler<AddCommunityStatisticsCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddCommunityStatisticsCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(AddCommunityStatisticsCommand command)
        {
            var communityStatistics = new CommunityStatisticsDbModel
            {
                AccountId = command.AccountId,
                GroupId = command.GroupId,
                CountOfPageInvitations = command.CountOfPageInvitations != null ? (long)command.CountOfPageInvitations : 0,
                CountOfGroupInvitations = command.CountOfGroupInvitations != null ? (long)command.CountOfGroupInvitations : 0,
                UpdateDateTime = DateTime.Now
            };

            _context.CommunityStatistics.AddOrUpdate(communityStatistics);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
