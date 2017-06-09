using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobState.DeleteState
{
    public class DeleteJobStateCommand : ICommand<List<string>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
