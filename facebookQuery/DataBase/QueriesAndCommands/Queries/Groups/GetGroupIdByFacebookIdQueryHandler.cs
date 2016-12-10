using System.Linq;
using System.Runtime.CompilerServices;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Groups
{
    public class GetGroupIdByFacebookIdQueryHandler : IQueryHandler<GetGroupIdByFacebookIdQuery, long?>
    {
        private readonly DataBaseContext context;

        public GetGroupIdByFacebookIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public long? Handle(GetGroupIdByFacebookIdQuery query)
        {
            var result = context
                .Accounts
                .Where(model => model.FacebookId == query.FacebookId && !model.IsDeleted)
                .Select(model => model.MessageGroupId)
                .FirstOrDefault();

            return result;
        }
    }
}
