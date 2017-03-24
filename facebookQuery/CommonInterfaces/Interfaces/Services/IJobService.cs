using CommonInterfaces.Interfaces.Models;

namespace CommonInterfaces.Interfaces.Services
{
    public interface IJobService
    {
        void AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model);

        void AddOrUpdateSpyAccountJobs(IAddOrUpdateAccountJobs model);

        void RemoveAccountJobs(IRemoveAccountJobs model);

        void RenameAccountJobs(IRenameAccountJobs model);
    }
}
