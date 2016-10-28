using RequestsHelpers;

namespace Engines.Engines.GetNewNoticesEngine
{
    public class GetNewNoticesEngine : AbstractEngine<GetNewNoticesModel, GetNewNoticesResponseModel>
    {
        protected override GetNewNoticesResponseModel ExecuteEngine(GetNewNoticesModel model)
        {
            var newFriends = int.Parse(ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "requestsCountValue"));
            var newMessages = int.Parse(ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "mercurymessagesCountValue"));
            var newNotices = int.Parse(ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "notificationsCountValue"));

            return new GetNewNoticesResponseModel()
            {
                NumberNewFriends = newFriends,
                NumberNewMessages = newMessages,
                NumberNewNotifications = newNotices
            };
        }
    }
}
