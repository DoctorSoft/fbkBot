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
            var jobStatus = _context.JobStatus
                .Where(model => model.AccountId == command.AccountId
                    && model.IsForSpy == command.IsForSpy
                    && model.FunctionName == command.FunctionName);

            if (command.FriendId !=null)
            {
                jobStatus = jobStatus.Where(model => model.FriendId == command.FriendId);
            }
            try
            {
                if (!jobStatus.Any())
                {
                    return new VoidCommandResponse();
                }

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
