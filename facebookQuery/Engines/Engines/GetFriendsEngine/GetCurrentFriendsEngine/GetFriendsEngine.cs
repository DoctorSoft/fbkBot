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

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsEngine
{
    public class GetFriendsEngine : AbstractEngine<GetFriendsModel, GetFriendsResponseModel>
    {
        protected override GetFriendsResponseModel ExecuteEngine(GetFriendsModel model)
        {
            if (model.UrlParameters == null) return null;
            
            var parametersDictionary = model.UrlParameters.ToDictionary(pair => (GetFriendsEnum)pair.Key, pair => pair.Value);

            parametersDictionary[GetFriendsEnum.Id] = model.AccountId.ToString("G");

            var parameters = CreateParametersString(parametersDictionary);

            var stringResponse = RequestsHelper.Get(Urls.GetFriends.GetDiscription() + parameters, model.Cookie, model.Proxy).Remove(0, 9);

            return GetFriendsData(stringResponse);
        }

        public static GetFriendsResponseModel GetFriendsData(string pageRequest)
        {
            var friendsList = new GetFriendsResponseModel();

            var regex = new Regex("id:\"*[^\")]*\",name:\"*[^\")]*\",firstName:\"*[^\")]*\",vanity:\"*[^\")]*\",thumbSrc:\"*[^\")]*\",uri:\"*[^\")]*\",gender:*[^\")]*,i");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);

            foreach (var friend in collection)
            {
                var a = friend.ToString().Remove(0, 4);
                var idRegex = new Regex("id:\"*[^\")]*\"");
                var id = idRegex.Match(friend.ToString()).ToString().Remove(0,4);

                var nameRegex = new Regex("name:\"*[^\")]*\"");
                var name = nameRegex.Match(friend.ToString()).ToString().Remove(0, 6);

                var uriRegex = new Regex("uri:\"*[^\")]*\"");
                var uri = uriRegex.Match(friend.ToString()).ToString().Remove(0, 5);

                var genderRegex = new Regex("gender:*[^\")]*");
                var genderString = genderRegex.Match(friend.ToString()).ToString().Remove(0, 7);
                var gender = Convert.ToInt32(genderString.Remove(genderString.Length - 2));

                friendsList.Friends.Add(new FriendsResponseModel
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
