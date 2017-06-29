using System.Collections.Generic;
using CommonInterfaces.Interfaces.Models;

namespace CommonInterfaces.Interfaces.Services.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model);
        
        void RemoveAllBackgroundJobs(IRemoveAccountJobs model);

        void RemoveBackgroundJobById(string jobId);

        bool CreateBackgroundJob(ICreateBackgroundJob model);
    }
}
