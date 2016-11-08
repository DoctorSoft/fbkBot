using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine
{
    public class GetFriendsEngine: AbstractEngine<GetFriendsModel, List<GetFriendsResponseModel>>
    {
        protected override List<GetFriendsResponseModel> ExecuteEngine(GetFriendsModel model)
        {
            var friendsList = new List<GetFriendsResponseModel>();

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetFriends
            });

            if (urlParameters == null) return null;

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie), "fb_dtsg");

            var parametersDictionary = urlParameters.ToDictionary(pair => (GetFriendsEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetFriendsEnum.Id] = model.AccountId.ToString("G");

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Get(Urls.GetFriends.GetDiscription()+ parameters, model.Cookie).Remove(0, 9);

            return GetFriendsData(stringResponse);
        }

        public static List<GetFriendsResponseModel> GetFriendsData(string pageRequest)
        {
            var regex = new Regex("id:\"*[^\")]*\",name:\"*[^\")]*\",firstName:\"*[^\")]*\"");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);

            foreach (var friend in collection)
            {
                var a = friend.ToString().Remove(0, 4);
                var s = a.IndexOf(",name)");
                var t = a.Length;
                
                var id = a.Remove(a.IndexOf("\",name)"), a.Length);
            }
            

            return null;
        }

        private static string CreateParametersString(Dictionary<GetFriendsEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
                }
            }

            return "?" + result.Remove(0, 1);
        }
    }
}
