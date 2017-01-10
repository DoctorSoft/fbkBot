using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.SpyFunctions;

namespace Services.ServiceTools
{
    public class FunctionPermissionManager
    {
        private readonly SpyAccountManager _spyAccountManager;

        public FunctionPermissionManager()
        {
            _spyAccountManager = new SpyAccountManager();
        }
        public bool HasPermissions(FunctionName functionName, long groupId)
        {
            var function = new GetGroupFunctionByIdQueryHandler(new DataBaseContext()).Handle(new GetGroupFunctionByIdQuery
            {
                GroupId = groupId,
                FunctionName = functionName
            });

            return function != null;
        }

        public bool HasPermissionsForSpy(FunctionName functionName, long spyAccountId)
        {
            var spyAccount = _spyAccountManager.GetSpyAccountByFacebookId(spyAccountId);

            var function = new GetSpyFunctionByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyFunctionByIdQuery
            {
                SpyId = spyAccount.Id,
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
