using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using Constants.UrlEnums;
using Engines.Engines.RemoveFriendEngine;
using RequestsHelpers;

namespace Engines.Engines.AddToGroupEngine
{
    public class AddToGroupEngine : AbstractEngine<AddToGroupModel, bool>
    {
        protected override bool ExecuteEngine(AddToGroupModel model)
        {
            try
            {
                if (model.UrlParameters == null)
                {
                    return false;
                }
                
                var fbDtsg = ParseResponsePageHelper.GetInputValueById(RequestsHelper.Get(Urls.HomePage.GetDiscription(), model.Cookie, model.Proxy), "fb_dtsg");
                var groupId = ParseResponsePageHelper.GetGroupId(RequestsHelper.Get(model.FacebookGroupUrl, model.Cookie, model.Proxy));

                var parametersDictionary = model.UrlParameters.ToDictionary(pair => (AddFriendsToGroupEnum)pair.Key, pair => pair.Value);

                parametersDictionary[AddFriendsToGroupEnum.User] = model.FacebookId.ToString(CultureInfo.InvariantCulture);
                parametersDictionary[AddFriendsToGroupEnum.FbDtsg] = fbDtsg;
                parametersDictionary[AddFriendsToGroupEnum.Members] = CreateString(model.FriendsList)[0];
                parametersDictionary[AddFriendsToGroupEnum.TextMembers] = CreateString(model.FriendsList)[1];

                var parameters = CreateParametersString(parametersDictionary);

                RequestsHelper.Post(string.Format(Urls.AddFriendToGroup.GetDiscription(), groupId), parameters, model.Cookie, model.Proxy);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static string CreateParametersString(Dictionary<AddFriendsToGroupEnum, string> parameters)
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
