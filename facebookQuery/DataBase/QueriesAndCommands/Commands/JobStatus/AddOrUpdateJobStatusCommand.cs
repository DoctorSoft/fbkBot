using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class AddOrUpdateJobStatusCommand : ICommand<long>
    {
        public FunctionName FunctionName { get; set; }

        public DateTime LaunchDateTime { get; set; }
        
    }
}
