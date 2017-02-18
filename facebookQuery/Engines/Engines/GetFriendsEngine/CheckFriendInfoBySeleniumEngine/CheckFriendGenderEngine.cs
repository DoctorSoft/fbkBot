using System;
using System.Text;
using System.Text.RegularExpressions;
using Constants.GendersUnums;
using Engines.Engines.GetFriendInfoEngine;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine
{
    public class CheckFriendGenderEngine : AbstractEngine<CheckFriendGenderModel, bool>
    {
        protected override bool ExecuteEngine(CheckFriendGenderModel model)
        {
            var contactStringResponse = RequestsHelper.Get("https://www.facebook.com/profile.php?id="
                                            + model.FriendFacebookId
                                            + "&sk=about"
                                            + "&lst=" + model.AccountFacebookId
                                            + "%3A" + model.FriendFacebookId
                                            + "%3A" + GenerateValue()
                                            + "&section=contact-info&pnref=about"
                                            , model.Cookie, model.Proxy);

            var gender = GetGender(contactStringResponse);

            return gender == model.Gender;
        }

        private static string GenerateValue()
        {
            var rand = new Random();

            return "1000101634" + rand.Next(10000, 99999);
        }
        
        public static GenderEnum? GetGender(string pageRequest)
        {

            var parentPattern = new Regex("<div class=\"clearfix _ikh\">.*?</div></div></div>");
            var parentCollection = parentPattern.Matches(pageRequest);

            foreach (var collection in parentCollection)
            {
                var convertCollection = ConvertToUTF8(collection.ToString());
                if (convertCollection.Contains("Пол") || convertCollection.Contains("Gender") || convertCollection.Contains("Sex"))
                {
                    var genderPattern = new Regex("_50f4\">.*?</");
                    var genderSring = genderPattern.Match(convertCollection).ToString().Remove(0, 7);

                    if (genderSring.Contains("Мужской"))
                    {
                        return GenderEnum.Male;
                    }

                    return GenderEnum.Female;
                }
            }

            return null;
        }

        private static string ConvertToUTF8(string source)
        {
            var utfBytes = Encoding.UTF8.GetBytes(source);
            var koi8RBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1251"), utfBytes);
            return Encoding.GetEncoding("utf-8").GetString(koi8RBytes);
        }
    }
}
