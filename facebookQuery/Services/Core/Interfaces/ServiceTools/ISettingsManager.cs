using Constants.FunctionEnums;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface ISettingsManager
    {
        bool HasARetryTimePermission(FunctionName functionName, AccountViewModel account);
    }
}
