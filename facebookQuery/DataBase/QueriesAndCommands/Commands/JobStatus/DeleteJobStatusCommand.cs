using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class DeleteJobStatusCommand : ICommand<VoidCommandResponse>
    {
        public FunctionName FunctionName { get; set; }
    }
}
