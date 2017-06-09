using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobState.AddState
{
    public class AddJobStateCommand : ICommand<VoidCommandResponse>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string JobId { get; set; }
    }
}
