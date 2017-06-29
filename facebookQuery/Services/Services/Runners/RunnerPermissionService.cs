using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Runners;
using DataBase.QueriesAndCommands.Queries.Runners.CheckPermissions;

namespace Services.Services.Runners
{
    public class RunnerPermissionService
    {
        public bool HasPermissions(long runnerId)
        {
            var isAllowed = new CheckRunnerPermissionsQueryHandler(new DataBaseContext()).Handle(new CheckRunnerPermissionsQuery
            {
                RunnerId = runnerId
            });

            return isAllowed;
        }
    }
}
