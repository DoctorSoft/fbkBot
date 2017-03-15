using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class AddJobStatusCommand : ICommand<long>
    {
        public long AccountId { get; set; }

        public string JobId { get; set; }

        public FunctionName FunctionName { get; set; }

        public TimeSpan LaunchDateTime { get; set; }
        
    }
}
