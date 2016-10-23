using DataBase.Constants;
using DataBase.QueriesAndCommands.Queries.UrlParameters.Models;

namespace DataBase.QueriesAndCommands.Queries.UrlParameters
{
    public class GetUrlParametersQuery: IQuery<IUrlParameters>
    {
        public NamesUrlParameter NameUrlParameter { get; set; }
    }
}
