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
            var result = new FriendInfoSection();

            if (model.Settings.LivesPlace != null || model.Settings.SchoolPlace != null || model.Settings.WorkPlace != null)
            {
                var stringResponse = RequestsHelper.Get("https://www.facebook.com/profile.php?id="
                                    + model.FriendFacebookId
                                    + "&sk=about"
                                    + "&lst=" + model.AccountFacebookId
                                    + "%3A" + model.FriendFacebookId
                                    + "%3A" + GenerateValue()
                                    + "&section=overview&pnref=about"
                                    , model.Cookie, model.Proxy);

                result = GetFriendsData(stringResponse);
            }
            
            if (model.Settings.Gender != null)
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

                result.Gender = gender;
            }

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

            var parentPattern = new Regex("class=\"_4bl7\"><ul([\\s\\S]*)</ul></div>");

            var parentCollection = parentPattern.Matches(pageRequest);
            if (parentCollection.Count == 0)
            {
                return null;
            }

            var parentCollectionString = parentCollection[0].ToString();

            var infoPattern = new Regex("<div class=\"clearfix.*?</div></div></div></div>");
            var infoCollection = infoPattern.Matches(parentCollectionString);

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
