using System;
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
            var jobStatus = _context.JobStatus.Where(model => model.AccountId == command.AccountId 
                && model.FunctionName == command.FunctionName);
            try
            {
                _context.JobStatus.RemoveRange(jobStatus);

                _context.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }

            return new VoidCommandResponse();
        }
    }
}
