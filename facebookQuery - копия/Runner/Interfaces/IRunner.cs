using Services.ViewModels.HomeModels;

namespace Runner.Interfaces
{
    public interface IRunner
    {
        void Run(AccountViewModel account);
    }
}
