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
            var queueModel = _context.JobsQueue.FirstOrDefault(model => model.Id == command.QueueId);

            _context.JobsQueue.Remove(queueModel);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
