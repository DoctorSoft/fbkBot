using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Settings
{
    public class JoinTheNewGroupRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new GroupService().JoinTheNewGroup(account);
        }
    }
}
