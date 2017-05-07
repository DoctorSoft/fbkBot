using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById
{
    public class GetUserAgentQueryHandler : IQueryHandler<GetUserAgentQuery, UserAgentData>
    {
        private readonly DataBaseContext _context;

        public GetUserAgentQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public UserAgentData Handle(GetUserAgentQuery query)
        {
            if (query.UserAgentId == null)
            {
                return null;
            }

            var userAgent = _context.UserAgents.FirstOrDefault(model => model.Id == query.UserAgentId);

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
