using System;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;

namespace DataBase.QueriesAndCommands.Queries.UrlParameters
{
    public class GetUrlParametersQueryHandler: IQueryHandler<GetUrlParametersQuery, IUrlParameters>
    {
        private readonly DataBaseContext context;

        public GetUrlParametersQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public IUrlParameters Handle(GetUrlParametersQuery query)
        {
            var urlParametersDbModel = context.UrlParameters.FirstOrDefault(model => model.CodeParameters == (int)query.NameUrlParameter);
            if (urlParametersDbModel == null) return null;
            var json = urlParametersDbModel.ParametersSet;
            if (query.NameUrlParameter != NamesUrlParameter.SendMessage) return null;
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<SendMessageUrlParametersModel>(json);
        }
    }
}
