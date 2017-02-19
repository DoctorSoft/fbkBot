using Services.ViewModels.HomeModels;

namespace Services.Interfaces
{
    public interface IJobService
    {
        void AddOrUpdateAccountJobs(AccountViewModel accountViewModel);

        void AddOrUpdateSpyAccountJobs(AccountViewModel accountViewModel);

        void RemoveAccountJobs(string login, long? accountId);

        void RenameAccountJobs(AccountViewModel accountViewModel, string oldLogin);
    }
}
