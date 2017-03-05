using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.ExtraMessages
{
    public class GetExtraMessagesQueryHandler : IQueryHandler<GetExtraMessagesQuery, List<ExtraMessagesData>>
    {
        private readonly DataBaseContext context;

        public GetExtraMessagesQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<ExtraMessagesData> Handle(GetExtraMessagesQuery query)
        {
            var extraMessages = context.ExtraMessages.Select(model => new ExtraMessagesData
            {
                Id = model.Id,
                Message = model.Message
            }).ToList();

            return extraMessages;
        }
    }
}
