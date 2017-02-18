using System;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue
{
    public class AddToQueueCommandHandler : ICommandHandler<AddToQueueCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public AddToQueueCommandHandler()
        {
            context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(AddToQueueCommand command)
        {
            var queueModel = new JobQueueDbModel
            {
                AccountId = command.AccountId,
                FunctionName = command.FunctionName,
                AddedDateTime = DateTime.Now
            };

            context.JobsQueue.Add(queueModel);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
