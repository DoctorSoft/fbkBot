using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueueByAccountId
{
    public class DeleteQueueByAccountIdCommand : ICommand<List<string>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName? FunctionName { get; set; }
    }
}
