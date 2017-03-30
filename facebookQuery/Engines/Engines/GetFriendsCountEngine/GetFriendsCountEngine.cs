using System;
using System.Linq;
using System.Text.RegularExpressions;
using Constants;
using Constants.EnumExtension;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsCountEngine
{
    public class GetFriendsCountEngine : AbstractEngine<GetFriendsCountModel, int>
    {
        protected override int ExecuteEngine(GetFriendsCountModel model)
        {
            var countFriends = GetFriendsCount(RequestsHelper.Get(Urls.GetFriends.GetDiscription(), model.Cookie, model.Proxy));

            return Convert.ToInt32(countFriends);
        }

        public static string GetFriendsCount(string pageRequest)
        {
            var regex = new Regex("class=\"_gs6\"[^<]*");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);
            var fullString = (from Match m in collection select m.Groups[0].Value).FirstOrDefault();

            var result = fullString != null ? fullString.Remove(0, 13) : "0";
            return result;
        }
    }
}
