using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Commands.JobStatus.DeleteJobStatusesByAccountId
{
    public class DeleteJobStatusesByAccountIdCommand : ICommand<List<string>>
    {
        public long AccountId { get; set; }
    }
}
