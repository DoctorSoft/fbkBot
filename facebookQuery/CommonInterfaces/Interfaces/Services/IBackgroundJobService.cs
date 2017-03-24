using CommonInterfaces.Interfaces.Models;

namespace CommonInterfaces.Interfaces.Services
{
    public interface IBackgroundJobService
    {
        bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model);

        void CreateBackgroundJob(ICreateBackgroundJob model);

        void CreateBackgroundJobForDeleteFriends(ICreateBackgroundJobForDeleteFriends model);
    }
}
