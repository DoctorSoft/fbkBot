using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;

namespace DataBase.QueriesAndCommands.Queries.UrlParameters
{
    public class GetUrlParametersQueryHandler : IQueryHandler<GetUrlParametersQuery, List<KeyValue<int, string>>>
    {
        private readonly DataBaseContext context;

        public GetUrlParametersQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<KeyValue<int, string>> Handle(GetUrlParametersQuery query)
        {
            var urlParametersDbModel = context.UrlParameters.FirstOrDefault(model => model.CodeParameters == (int)query.NameUrlParameter);
            if (urlParametersDbModel == null) return null;
            var json = urlParametersDbModel.ParametersSet;
            var serializer = new JavaScriptSerializer();
            var answer = serializer.Deserialize<List<KeyValue<int, string>>>(json);
            return answer;
        }
    }
}
