using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Constants;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.UrlParameters
{
    public class GetUrlParametersQueryHandler : IQueryHandler<GetUrlParametersQuery, List<KeyValue<int, string>>>
    {
        private readonly DataBaseContext _context;

        public GetUrlParametersQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<KeyValue<int, string>> Handle(GetUrlParametersQuery query)
        {
            var urlParametersDbModel = _context.UrlParameters.FirstOrDefault(model => model.CodeParameters == (int)query.NameUrlParameter);
            if (urlParametersDbModel == null) return null;
            var json = urlParametersDbModel.ParametersSet;
            var serializer = new JavaScriptSerializer();
            var answer = serializer.Deserialize<List<KeyValue<int, string>>>(json);
            return answer;
        }
    }
}
