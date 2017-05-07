using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using Engines.Engines.AddToGroupEngine;
using RequestsHelpers;

namespace Engines.Engines.AddToPageEngine
{
    public class AddToPageEngine : AbstractEngine<AddToPageModel, bool>
    {
        protected override bool ExecuteEngine(AddToPageModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent), "fb_dtsg");
                var pageId = ParseResponsePageHelper.GetGroupId(RequestsHelper.Get(model.FacebookPageUrl, model.Cookie, model.Proxy, model.UserAgent));

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (AddFriendsToPageEnum)pair.Key, pair => pair.Value);

                parametersDictionary[AddFriendsToPageEnum.User] = model.FacebookId.ToString(CultureInfo.InvariantCulture);
                parametersDictionary[AddFriendsToPageEnum.FbDtsg] = fbDtsg;
                parametersDictionary[AddFriendsToPageEnum.PageId] = pageId;
                parametersDictionary[AddFriendsToPageEnum.Invitee] = model.Friend.FacebookId.ToString(CultureInfo.InvariantCulture);

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(Urls.AddFriendToPage.GetDiscription(), parameters, model.Cookie, model.Proxy, model.UserAgent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<AddFriendsToPageEnum, string> parameters)
        {
            var result = "";
            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    result += "&" + parameter.Key.GetAttributeName() + parameter.Value;
                }
            }

            return result.Remove(0, 1);
        }

        private List<string> CreateString(List<FriendModel> friendModels)
        {
            int i = 0;
            var members = string.Empty;
            var textMembers = string.Empty;

            foreach (var friendModel in friendModels)
            {
                if (i == 0)
                {
                    if (i == friendModels.Count() - 1)
                    {
                        members += string.Format("members[{0}]={1}", i, friendModel.FacebookId);
                        textMembers += string.Format("text_members[{0}]={1}", i, friendModel.FriendName);

                        i++;
                        continue;

                    }

                    members += string.Format("{0}&", friendModel.FacebookId); 
                    textMembers += string.Format("{0}&", friendModel.FriendName);

                    i++;
                    continue;
                }

                if (i == friendModels.Count())
                {
                    members += string.Format("members[{0}]={1}", i, friendModel.FacebookId);
                    textMembers += string.Format("text_members[{0}]={1}", i, friendModel.FriendName);

                    i++;
                    continue;

                }

                members += string.Format("members[{0}]={1}&", i, friendModel.FacebookId);
                textMembers += string.Format("text_members[{0}]={1}&", i, friendModel.FriendName);

                i++;
            }

            return new List<string>
            {
                members, 
                textMembers
            };
        }
    }
}
