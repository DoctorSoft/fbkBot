using CommonInterfaces.Interfaces.Models;

namespace CommonInterfaces.Interfaces.Services
{
    public interface IBackgroundJobService
    {
        bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model);

        void RemoveJobById(string jobId);
        
        void CreateBackgroundJob(ICreateBackgroundJob model);
    }
}
