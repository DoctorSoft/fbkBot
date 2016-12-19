using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using Constants.FriendTypesEnum;
using Constants.GendersUnums;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine
{
    public class GetRecommendedFriendsEngine : AbstractEngine<GetRecommendedFriendsModel, List<GetFriendsResponseModel>>
    {
        protected override List<GetFriendsResponseModel> ExecuteEngine(GetRecommendedFriendsModel model)
        {
            var stringResponse = RequestsHelper.Get(Urls.GetRecommendedFriends.GetDiscription(), model.Cookie, model.Proxy).Remove(0, 9);

            return GetFriendsData(stringResponse);
        }

        public static List<GetFriendsResponseModel> GetFriendsData(string pageRequest)
        {
            var friendsList = new List<GetFriendsResponseModel>();

            var incomingPattern = new Regex("clearfix ruUserBox _3-z friendRequestItem\".*?</div></div></div></div></div>");
            var recommendedPattern = new Regex("friendBrowserListUnit\".*?</div></div></div></div></div>");

            var incomingCollection = incomingPattern.Matches(pageRequest);
            var recommendedCollection = recommendedPattern.Matches(pageRequest);

            foreach (var incomingFriend in incomingCollection)
            {
                var firstString = new Regex("user.php.*?</a></div>");
                var dataStep1 = firstString.Match(incomingFriend.ToString()).ToString().Remove(0, 12);
                var dataStep2 = dataStep1.Remove(dataStep1.Length - 10);

                var index1 = dataStep2.IndexOf("\" data", StringComparison.Ordinal);
                var id = dataStep2.Remove(index1);

                var index2 = dataStep2.IndexOf(">", StringComparison.Ordinal);
                var name = dataStep2.Remove(0, index2 + 1);

                friendsList.Add(new GetFriendsResponseModel()
                {
                    FacebookId = Convert.ToInt64(id.Remove(id.Length-1)),
                    FriendName = ConvertToUTF8(name.Remove(name.Length - 1)),
                    Type = FriendTypes.Incoming
                });
            }

            foreach (var recommendedFriend in recommendedCollection)
            {
                var firstString = new Regex("user.php.*?</a></div>");
                var dataStep1 = firstString.Match(recommendedFriend.ToString()).ToString().Remove(0, 12);
                var dataStep2 = dataStep1.Remove(dataStep1.Length - 10);

                var index1 = dataStep2.IndexOf("&amp;", StringComparison.Ordinal);
                var id = dataStep2.Remove(index1);

                var index2 = dataStep2.IndexOf(">", StringComparison.Ordinal);
                var name = dataStep2.Remove(0, index2 + 1);

                friendsList.Add(new GetFriendsResponseModel()
                {
                    FacebookId = Convert.ToInt64(id.Remove(id.Length - 1)),
                    FriendName = ConvertToUTF8(name.Remove(name.Length - 1)),
                    Type = FriendTypes.Recommended
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
    }
}
