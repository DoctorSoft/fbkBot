using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Links
{
    public class GetLinksQueryHandler : IQueryHandler<GetLinksQuery, List<LinkData>>
    {
        private readonly DataBaseContext context;

        public GetLinksQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<LinkData> Handle(GetLinksQuery query)
        {
            var links = context.Links.Select(model => new LinkData
            {
                Id = model.Id,
                Name = model.Link
            }).ToList();

            return links;
        }
    }
}
