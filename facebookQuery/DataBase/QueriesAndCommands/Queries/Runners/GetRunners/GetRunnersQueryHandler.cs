using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Runners.GetRunners
{
    public class GetRunnersQueryHandler : IQueryHandler<GetRunnersQuery, List<RunnerModel>>
    {
        private readonly DataBaseContext _context;

        public GetRunnersQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<RunnerModel> Handle(GetRunnersQuery query)
        {
            try
            {
                var runners = _context.Runners.Select(model => new RunnerModel
                {
                    IsAllowed = model.IsAllowed,
                    DeviceName = model.DeviceName,
                    LastAction = model.LastAction,
                    Id = model.Id,
                    CreateDate = model.CreateDate
                }).ToList();

                return runners;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
