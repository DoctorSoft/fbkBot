using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class DeleteJobStatusCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }
    }
}
