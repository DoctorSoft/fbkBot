using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupsQueryHandler : IQueryHandler<GetGroupsQuery, List<GroupData>>
    {
        private readonly DataBaseContext context;

        public GetGroupsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<GroupData> Handle(GetGroupsQuery query)
        {
            var groups = context.MessageGroups.Select(model => new GroupData
            {
                Id = model.Id,
                Name = model.Name
            }).ToList();

            return groups;
        }
    }
}
