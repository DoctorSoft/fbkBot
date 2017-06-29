using System;
using RequestsHelpers;
using System.Text;

namespace Engines.Engines.AddToPageEngine
{
    public class BotPageIsWorkEngine : AbstractEngine<BotPageIsWorkModel, bool>
    {
        protected override bool ExecuteEngine(BotPageIsWorkModel model)
        {
            try
            {
                var resultUrl = string.Format("https://www.facebook.com/profile.php?id={0}", model.FriendFacebookId);

                var page = RequestsHelper.Get(resultUrl, model.Cookie, model.Proxy, model.UserAgent);
                var utf8Page = ConvertToUTF8(page);

                var result = CheckPage(utf8Page);

                return result;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string ConvertToUTF8(string source)
        {
            var utfBytes = Encoding.UTF8.GetBytes(source);
            var koi8RBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utfBytes);
            return Encoding.GetEncoding("utf-8").GetString(koi8RBytes);
        }

        private bool CheckPage(string page)
        {
            return !page.Contains("Sorry, this content isn't available right now") && !page.Contains("К сожалению, эти материалы сейчас недоступны");
        }
    }
}
