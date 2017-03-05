using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class DeleteJobStatusCommandHandler : ICommandHandler<DeleteJobStatusCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteJobStatusCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(DeleteJobStatusCommand command)
        {
            var jobStatus = _context.JobStatus.FirstOrDefault(model => model.AccountId == command.AccountId);
            
            _context.JobStatus.Remove(jobStatus);

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
