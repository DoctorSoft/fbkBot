﻿using System;
using System.Linq;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue
{
    public class AddToQueueCommandHandler : ICommandHandler<AddToQueueCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public AddToQueueCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(AddToQueueCommand command)
        {
            var queueModel = new JobQueueDbModel
            {
                AccountId = command.AccountId,
                FunctionName = command.FunctionName,
                AddedDateTime = DateTime.Now,
                FriendId = command.FriendId
            };

            if (command.IsUnique)
            {
                var queueInDb = _context.JobsQueue.Any(model => model.AccountId == command.AccountId && model.FriendId == command.FriendId && model.FunctionName == command.FunctionName);
                if (queueInDb)
                {
                    return new VoidCommandResponse();
                }
            }
            _context.JobsQueue.Add(queueModel);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
