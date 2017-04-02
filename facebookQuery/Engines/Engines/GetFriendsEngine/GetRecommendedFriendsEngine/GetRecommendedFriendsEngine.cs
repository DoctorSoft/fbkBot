using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using Constants.FriendTypesEnum;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine
{
    public class GetRecommendedFriendsEngine : AbstractEngine<GetRecommendedFriendsModel, GetFriendsResponseModel>
    {
        protected override GetFriendsResponseModel ExecuteEngine(GetRecommendedFriendsModel model)
        {
            var stringResponse = RequestsHelper.Get(Urls.GetRecommendedFriends.GetDiscription(), model.Cookie, model.Proxy).Remove(0, 9);

            return GetFriendsData(stringResponse);
        }

        public static GetFriendsResponseModel GetFriendsData(string pageRequest)
        {
            var friendsList = new GetFriendsResponseModel
            {
                Friends = new List<FriendsResponseModel>()
            };

            var incomingPattern = new Regex("clearfix ruUserBox _3-z friendRequestItem\".*?</div></div></div></div></div>");
            var recommendedFriendsPattern = new Regex("clearfix ruUserBox _3-z\".*?</div></div></div></div>");
            var recommendedPattern = new Regex("friendBrowserListUnit\".*?</div></div></div></div>");

            var incomingCollection = incomingPattern.Matches(pageRequest);
            var recommendedFriendsCollection = recommendedFriendsPattern.Matches(pageRequest);
            var recommendedCollection = recommendedPattern.Matches(pageRequest);

            friendsList.CountIncommingFriends = incomingCollection.Count;

            foreach (var incomingFriend in incomingCollection)
            {
                var firstString = new Regex("user.php.*?</a></div>");
                var dataStep1 = firstString.Match(incomingFriend.ToString()).ToString().Remove(0, 12);
                var dataStep2 = dataStep1.Remove(dataStep1.Length - 10);

                var index1 = dataStep2.IndexOf("\" data", StringComparison.Ordinal);
                var id = dataStep2.Remove(index1);

                var index2 = dataStep2.IndexOf(">", StringComparison.Ordinal);
                var name = dataStep2.Remove(0, index2 + 1);

                friendsList.Friends.Add(new FriendsResponseModel
                {
                    FacebookId = Convert.ToInt64(id),
                    FriendName = ConvertToUTF8(name.Remove(name.Length - 1)),
                    Type = FriendTypes.Incoming
                });
            }

            foreach (var recommendedFriend in recommendedFriendsCollection)
            {
                var firstString = new Regex("user.php.*?</a></div>");
                var dataStep1 = firstString.Match(recommendedFriend.ToString()).ToString().Remove(0, 12);
                var dataStep2 = dataStep1.Remove(dataStep1.Length - 10);

                var index1 = dataStep2.IndexOf("\" data", StringComparison.Ordinal);
                var id = dataStep2.Remove(index1);

                var index2 = dataStep2.IndexOf(">", StringComparison.Ordinal);
                var name = dataStep2.Remove(0, index2 + 1);

                var index3 = name.IndexOf("<", StringComparison.Ordinal);
                if (index3 != -1)
                {
                    name = name.Remove(index3);
                }

                friendsList.Friends.Add(new FriendsResponseModel
                {
                    FacebookId = Convert.ToInt64(id),
                    FriendName = ConvertToUTF8(name.Remove(name.Length - 1)),
                    Type = FriendTypes.Recommended
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

                var index3 = name.IndexOf("<", StringComparison.Ordinal); 
                if (index3 != -1)
                {
                    name = name.Remove(index3);
                }

                friendsList.Friends.Add(new FriendsResponseModel
                {
                    FacebookId = Convert.ToInt64(id),
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
