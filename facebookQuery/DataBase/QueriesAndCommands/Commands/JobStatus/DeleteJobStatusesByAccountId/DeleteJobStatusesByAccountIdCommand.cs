using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobStatus.DeleteJobStatusesByAccountId
{
    public class DeleteJobStatusesByAccountIdCommand : ICommand<List<string>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName? FunctionName { get; set; }
    }
}
