using System;
using System.Linq;
using System.Web.Script.Serialization;
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
            var jsSerializator = new JavaScriptSerializer();
            var launchTimeJson = jsSerializator.Serialize(command.LaunchDateTime);

            var queueModel = new JobQueueDbModel
            {
                AccountId = command.AccountId,
                FunctionName = command.FunctionName,
                AddedDateTime = DateTime.Now,
                FriendId = command.FriendId,
                IsForSpy = command.IsForSpy,
                LaunchDateTime = launchTimeJson,
                JobId = command.JobId
            };

            if (command.IsUnique)
            {
                var queueInDb = _context.JobsQueue.Any(model => model.AccountId == command.AccountId
                    && model.FriendId == command.FriendId
                    && model.IsForSpy == command.IsForSpy 
                    && model.FunctionName == command.FunctionName);
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
