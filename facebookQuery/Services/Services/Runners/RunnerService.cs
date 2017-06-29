using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Runners;
using DataBase.QueriesAndCommands.Commands.Runners.DeleteRunners;
using DataBase.QueriesAndCommands.Queries.Runners.GetRunners;
using Services.ViewModels.RunnerModels;

namespace Services.Services.Runners
{
    public class RunnerService
    {
        public long AddRunner(RunnerViewModel model)
        {
            var result = new AddRunnerCommandHandler(new DataBaseContext()).Handle(new  AddRunnerCommand
            {
                DeviceName = model.DeviceName,
                IsAllowed = true
            });

            return result;
        }

        public void UpdateRunnerActivityDate(long runnerId)
        {
            new UpdateRunnerActivityDateCommandHandler(new DataBaseContext()).Handle(new UpdateRunnerActivityDateCommand
            {
                RunnerId = runnerId
            });
        }

        public void UpdateRunner(long runnerId, bool isAllowed)
        {
            if (isAllowed)
            {
                UpdateRunnerActivityDate(runnerId);
            }

            new UpdateRunnerCommandHandler(new DataBaseContext()).Handle(new UpdateRunnerCommand
            {
                RunnerId = runnerId,
                IsAllowed = isAllowed
            });
        }

        public List<RunnerViewModel> GetRunners()
        {
            var runners = new GetRunnersQueryHandler(new DataBaseContext()).Handle(new GetRunnersQuery());
            var result = runners.Select(model => new RunnerViewModel
            {
                IsAllowed = model.IsAllowed,
                DeviceName = model.DeviceName,
                LastAction = model.LastAction,
                Id = model.Id,
                CreateDate = model.CreateDate
            }).ToList();

            return result;
        }

        public void DeleteOldRunners()
        {
            new DeleteRunnersCommandHandler().Handle(new DeleteRunnersCommand{ OverdueMin = 1 });
        }
    }
}
