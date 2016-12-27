using System;
using System.Text;
using System.Text.RegularExpressions;
using Constants.FriendInfoEnums;
using Constants.GendersUnums;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsEngine;
using RequestsHelpers;

namespace Engines.Engines.GetFriendInfoEngine
{
    public class GetFriendInfoEngine : AbstractEngine<GetFriendInfoModel, FriendInfoSection>
    {
        protected override FriendInfoSection ExecuteEngine(GetFriendInfoModel model)
        {
            var stringResponse = RequestsHelper.Get("https://www.facebook.com/profile.php?id=" 
                                                + model.FrienFacebookId
                                                + "&sk=about"
                                                + "&lst=" + model.AccountFacebookId 
                                                + "%3A" + model.FrienFacebookId 
                                                + "%3A" + GenerateValue() 
                                                + "&section=overview&pnref=about"
                                                , model.Cookie, model.Proxy);

            var result = GetFriendsData(stringResponse);

            if (!model.GetGenderFunctionEnable)
            {
                return result;
            }

            var contactStringResponse = RequestsHelper.Get("https://www.facebook.com/profile.php?id="
                                                           + model.FrienFacebookId
                                                           + "&sk=about"
                                                           + "&lst=" + model.AccountFacebookId
                                                           + "%3A" + model.FrienFacebookId
                                                           + "%3A" + GenerateValue()
                                                           + "&section=contact-info&pnref=about"
                                                           , model.Cookie, model.Proxy);

            var gender = GetGender(contactStringResponse);

            result.Gender = gender;

            return result;
        }

        private static string GenerateValue()
        {
            var rand = new Random();

            return "1000101634" + rand.Next(10000, 99999);
        }

        public static FriendInfoSection GetFriendsData(string pageRequest)
        {
            var friendData = new FriendInfoSection();

            var parentPattern = new Regex("class=\"_4bl7\"><ul.*?</ul></div>");
            var parentCollection = parentPattern.Matches(pageRequest)[0].ToString();

            var infoPattern = new Regex("<div class=\"clearfix.*?</div></div></div></div>");
            var infoCollection = infoPattern.Matches(parentCollection);

            var i = 1;

            foreach (var section in infoCollection)
            {
                var fillingSectionPattern = new Regex("data-overviewsection=.*?</div></div></div></div>");
                var fillingSection = fillingSectionPattern.Match(section.ToString()).ToString();

                if (fillingSection != String.Empty)
                {
                    //var linksPattern = new Regex("<a class=\"profileLink\".*?</a|>[^<].*?<a class=\"profileLink\".*?</a");
                    //var links = linksPattern.Matches(fillingSection);

                    switch (i)
                    {
                        case (int)FriendInfoSections.Work:
                            friendData.WorkSection = ConvertToUTF8(fillingSection);
                            break;
                        case (int)FriendInfoSections.School:
                            friendData.SchoolSection = ConvertToUTF8(fillingSection);
                            break;
                        case (int)FriendInfoSections.Live:
                            friendData.LivesSection = ConvertToUTF8(fillingSection);
                            break;
                        case (int)FriendInfoSections.Relations:
                            friendData.RelationsSection = ConvertToUTF8(fillingSection);
                            break;
                    }
                }

                i++;
            }

            return friendData;
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
