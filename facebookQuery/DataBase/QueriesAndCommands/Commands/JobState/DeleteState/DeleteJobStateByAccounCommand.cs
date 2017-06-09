using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Commands.JobState.DeleteState
{
    public class DeleteJobStateByAccounCommand : ICommand<List<string>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }
    }
}
