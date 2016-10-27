using RequestsHelpers;

namespace Engines.Engines.GetAccountStatusEngine
{
    public class GetAccountStatusEngine : AbstractEngine<GetAccountStatusModel, GetAccountStatusResponseModel>
    {
        protected override GetAccountStatusResponseModel ExecuteEngine(GetAccountStatusModel model)
        {
            var newFriends = ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "requestsCountValue");
            var newMessages = ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "mercurymessagesCountValue");
            var newNotices = ParseResponsePageHelper.GetSpanValueById(model.ResponsePage, "notificationsCountValue");

            return new GetAccountStatusResponseModel()
            {
                NumberNewFriends = newFriends,
                NumberNewMessages = newMessages,
                NumberNewNotifications = newNotices
            };
        }
    }
}
