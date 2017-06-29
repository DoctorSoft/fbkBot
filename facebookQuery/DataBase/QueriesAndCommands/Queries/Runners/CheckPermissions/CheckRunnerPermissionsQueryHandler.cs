using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Runners.CheckPermissions
{
    public class CheckRunnerPermissionsQueryHandler : IQueryHandler<CheckRunnerPermissionsQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckRunnerPermissionsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(CheckRunnerPermissionsQuery query)
        {
            try
            {
                var runner = _context.Runners.FirstOrDefault(model => model.Id == query.RunnerId);

                return runner != null && runner.IsAllowed;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
