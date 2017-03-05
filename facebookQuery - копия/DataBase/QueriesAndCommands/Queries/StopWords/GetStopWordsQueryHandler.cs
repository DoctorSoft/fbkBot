using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.StopWords
{
    public class GetStopWordsQueryHandler : IQueryHandler<GetStopWordsQuery, List<StopWordData>>
    {
        private readonly DataBaseContext context;

        public GetStopWordsQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<StopWordData> Handle(GetStopWordsQuery query)
        {
            var groups = context.StopWords.Select(model => new StopWordData
            {
                Id = model.Id,
                Name = model.Word
            }).ToList();

            return groups;
        }
    }
}
