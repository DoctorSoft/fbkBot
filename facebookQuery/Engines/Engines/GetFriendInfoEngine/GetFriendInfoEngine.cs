using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsEngine;
using RequestsHelpers;

namespace Engines.Engines.GetFriendInfoEngine
{
    public class GetFriendInfoEngine : AbstractEngine<GetFriendInfoModel, FriendInfoData>
    {
        protected override FriendInfoData ExecuteEngine(GetFriendInfoModel model)
        {
            var stringResponse = RequestsHelper.Get("https://www.facebook.com/profile.php?id=" 
                + model.FrienFacebookId
                + "&sk=about"
                + "&lst=" + model.AccountFacebookId 
                + "%3A" + model.FrienFacebookId 
                + "%3A" + GenerateValue() 
                + "&section=overview&pnref=about"
                , model.Cookie, model.Proxy);

            GetFriendsData(stringResponse);
       // https://www.facebook.com/profile.php?id=100009193612085&lst=100013726390504%3A100009193612085%3A1482348446&sk=about
            return null;
        }

        private static string GenerateValue()
        {
            var rand = new Random();

            return "1000101634" + rand.Next(10000, 99999);
        }

        public static List<FriendInfoData> GetFriendsData(string pageRequest)
        {
            var friendsList = new List<FriendInfoData>();

            var parentPattern = new Regex("<!-- <ul class=\"uiList.*?--></code>");
            var parentCollection = parentPattern.Matches(pageRequest)[0].ToString();

            var infoPattern = new Regex("<div class=\"clearfix.*?</div></div></div></div></div>");
            var infoCollection = infoPattern.Matches(parentCollection);

//            foreach (var incomingFriend in incomingCollection)
//            {
//                var firstString = new Regex("user.php.*?</a></div>");
//                var dataStep1 = firstString.Match(incomingFriend.ToString()).ToString().Remove(0, 12);
//                var dataStep2 = dataStep1.Remove(dataStep1.Length - 10);
//
//                var index1 = dataStep2.IndexOf("\" data", StringComparison.Ordinal);
//                var id = dataStep2.Remove(index1);
//
//                var index2 = dataStep2.IndexOf(">", StringComparison.Ordinal);
//                var name = dataStep2.Remove(0, index2 + 1);
//
//                friendsList.Add(new FriendInfoData()
//                {
//                });
//            }

            return null;
        }
    }
}
