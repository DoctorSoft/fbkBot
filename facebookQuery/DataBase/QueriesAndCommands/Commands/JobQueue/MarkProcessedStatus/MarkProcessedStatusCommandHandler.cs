using System;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.MarkProcessedStatus
{
    public class MarkProcessedStatusCommandHandler : ICommandHandler<MarkProcessedStatusCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public MarkProcessedStatusCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(MarkProcessedStatusCommand command)
        {
            var queueInDb = _context.JobsQueue.FirstOrDefault(model => model.AccountId == command.AccountId
                && model.FriendId == command.FriendId
                && model.IsForSpy == command.IsForSpy 
                && model.FunctionName == command.FunctionName);

            if (queueInDb != null)
            {
                queueInDb.IsProcessed = true;
            }
            _context.JobsQueue.AddOrUpdate(queueInDb);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
