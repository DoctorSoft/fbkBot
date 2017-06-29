using System.Collections.Generic;

namespace Constants.FunctionEnums
{
    public class RequiredFunctions
    {
        private readonly List<FunctionName> _requiredFunctions = new List<FunctionName>
        {
            FunctionName.RefreshCookies,
            FunctionName.CheckFriendsAtTheEndTimeConditions
        };

        public List<FunctionName> GetRequiredFunctions()
        {
            return _requiredFunctions;
        }
    }
}
