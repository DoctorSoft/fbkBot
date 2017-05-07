using System;
using System.Text.RegularExpressions;
using System.Threading;
using Constants;
using Constants.EnumExtension;
using RequestsHelpers;

namespace Engines.Engines.GetNewWinks
{
    public class GetNewWinksEngine : AbstractEngine<GetNewWinksModel, int>
    {
        protected override int ExecuteEngine(GetNewWinksModel model)
        {
            var page = RequestsHelper.Get(Urls.GetNewWinks.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent);

            var countNewPokes = GetFriendsCount(page, model, model.UserAgent);

            return countNewPokes;
        }

        public static int GetFriendsCount(string pageRequest, GetNewWinksModel model, string userAgent)
        {
            var newPokes = new Regex("<div id=\"poke_live_new\">(.*?)class=\"_4-u2 _xct _4-u8\">");
            if (!newPokes.IsMatch(pageRequest))
            {
                newPokes = new Regex("<div id=\"poke_live_new\">(.*?)bottomContent");
                if (!newPokes.IsMatch(pageRequest))
                {
                    return 0;
                }
            }
            var blockPokes = newPokes.Match(pageRequest).ToString();

            var regex = new Regex("<a class=\"_42ft _4jy0 _4jy3 _4jy1 selected _51sy\" role=\"button\" href=\"#\" ajaxify=\"[^\"]*");
            if (!regex.IsMatch(pageRequest))
            {
                return 0;
            }

            var collection = regex.Matches(blockPokes);

            foreach (var links in collection)
            {
                var link = links != null ? links.ToString().Remove(0, 88) : "";

                const string substr = "amp;";
                link = link.Replace(substr, "");

                var fullLink = Urls.GetNewWinks.GetDiscription() + link;
                RequestsHelper.Get(fullLink, model.Cookie, model.Proxy, userAgent);

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }

            return collection.Count;
        }
    }
}
