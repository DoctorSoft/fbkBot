using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class DeleteJobStatusCommandHandler : ICommandHandler<DeleteJobStatusCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public DeleteJobStatusCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(DeleteJobStatusCommand command)
        {
            var jobStatus = context.JobStatus.FirstOrDefault(model => model.FunctionName == command.FunctionName);
            
            context.JobStatus.Remove(jobStatus);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
