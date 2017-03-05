using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using Constants.GendersUnums;
using Constants.UrlEnums;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsByCriteriesEngine
{
    public class GetFriendsByCriteriesEngine : AbstractEngine<GetFriendsByCriteriesModel, List<GetFriendsResponseModel>>
    {
        protected override List<GetFriendsResponseModel> ExecuteEngine(GetFriendsByCriteriesModel model)
        {
            if (model.UrlParameters == null)
            {
                return null;
            }

            var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy), "fb_dtsg");

            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetFriendsByCriteriesEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetFriendsByCriteriesEnum.User] = model.AccountId.ToString("G");
            parametersDictionary[GetFriendsByCriteriesEnum.FbDtsg] = fbDtsg;
            parametersDictionary[GetFriendsByCriteriesEnum.HomeTownIdsZero] = "115085015172389";

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Post(Urls.GetFriendsByCriteries.GetDiscription(), parameters, model.Cookie, model.Proxy).Remove(0, 9);

            return GetFriendsData(stringResponse);
        }

        public static List<GetFriendsResponseModel> GetFriendsData(string pageRequest)
        {
            var friendsList = new List<GetFriendsResponseModel>();

            var regex = new Regex("<li class=\"friendBrowserListUnit\".*?</li>");
            if (!regex.IsMatch(pageRequest))
            {
                return null;
            }

            var collection = regex.Matches(pageRequest);

            foreach (var friend in collection)
            {
                var idRegex = new Regex("name=\"friend_browser_id.*?value=\".*?\"");
                var id = idRegex.Match(friend.ToString()).ToString().Remove(0,4);

                var nameRegex = new Regex("data-hovercard-prefer-more-content-show=\"1\">.*?<");
                var name = nameRegex.Match(friend.ToString()).ToString().Remove(0, 6);

                var uriRegex = new Regex("uri:\"*[^\")]*\"");
                var uri = uriRegex.Match(friend.ToString()).ToString().Remove(0, 5);

                var genderRegex = new Regex("gender:*[^\")]*");
                var genderString = genderRegex.Match(friend.ToString()).ToString().Remove(0, 7);
                var gender = Convert.ToInt32(genderString.Remove(genderString.Length - 2));

                friendsList.Add(new GetFriendsResponseModel()
                {
                    FacebookId = Convert.ToInt64(id.Remove(id.Length-1)),
                    FriendName = ConvertToUTF8(name.Remove(name.Length - 1)),
                    Gender = gender == 1 ? GenderEnum.Female : GenderEnum.Male,
                    Uri = uri.Remove(uri.Length - 1)
                });
            }


            return friendsList;
        }
        private static string ConvertToUTF8(string source)
        {
            var utfBytes = Encoding.UTF8.GetBytes(source);
            var koi8RBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utfBytes);
            return Encoding.GetEncoding("utf-8").GetString(koi8RBytes);
        }

        private static string CreateParametersString(Dictionary<GetFriendsByCriteriesEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
                }
            }

            return result.Remove(0, 1);
        }
    }
}
