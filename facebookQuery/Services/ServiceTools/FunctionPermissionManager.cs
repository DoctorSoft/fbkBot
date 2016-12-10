using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Groups;

namespace Services.ServiceTools
{
    public class FunctionPermissionManager
    {
        public bool HasPermissions(FunctionName functionName, long groupId)
        {
            var function = new GetGroupFunctionByIdQueryHandler(new DataBaseContext()).Handle(new GetGroupFunctionByIdQuery
            {
                GroupId = groupId,
                FunctionName = functionName
            });

            return function != null;
        }

        public bool HasPermissionsByFacebookId(FunctionName functionName, long facebookId)
        {
            var groupId = new GetGroupIdByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetGroupIdByFacebookIdQuery
            {
                FacebookId = facebookId
            });

            if (groupId == null)
            {
                return true;
            }

            return HasPermissions(functionName, groupId.Value);
        }
    }
}
