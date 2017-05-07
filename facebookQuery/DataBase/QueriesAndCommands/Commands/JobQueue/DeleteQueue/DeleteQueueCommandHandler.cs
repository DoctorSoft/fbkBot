using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueue
{
    public class DeleteQueueCommandHandler : ICommandHandler<DeleteQueueCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteQueueCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(DeleteQueueCommand command)
        {
            try
            {
                var queueModel = _context.JobsQueue.FirstOrDefault(model => model.Id == command.QueueId);
                if (queueModel == null)
                {
                    return new VoidCommandResponse();
                }
                _context.JobsQueue.Remove(queueModel);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }

            return new VoidCommandResponse();
        }
    }
}
