using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Commands.Settings;

namespace DataBase.QueriesAndCommands.Commands.JobStatus
{
    public class AddOrUpdateJobStatusCommandHandler : ICommandHandler<AddOrUpdateJobStatusCommand, long>
    {
        private readonly DataBaseContext context;

        public AddOrUpdateJobStatusCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long Handle(AddOrUpdateJobStatusCommand command)
        {
            var jobStatus = context.JobStatus.FirstOrDefault(model => model.FunctionName == command.FunctionName) ??
                            new JobStatusDbModel();

            jobStatus.LastLaunchDateTime = command.LaunchDateTime;

            context.JobStatus.AddOrUpdate(jobStatus);

            context.SaveChanges();

            return jobStatus.Id;
        }
    }
}
