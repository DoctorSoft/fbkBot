using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.UserAgent.GetRandomUserAgent
{
    public class GetRandomUserAgentQueryHandler : IQueryHandler<GetRandomUserAgentQuery, UserAgentData>
    {
        private readonly DataBaseContext _context;

        public GetRandomUserAgentQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public UserAgentData Handle(GetRandomUserAgentQuery query)
        {
            var userAgent = _context.UserAgents.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            if (userAgent == null)
            {
                return null;
            }

            return new UserAgentData
            {
                Id = userAgent.Id,
                CreateDate = userAgent.CreateDate,
                UserAgentString = userAgent.UserAgentString
            };
        }
    }
}
