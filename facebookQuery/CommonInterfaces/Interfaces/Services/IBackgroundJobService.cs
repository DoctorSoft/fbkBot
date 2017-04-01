using System.Collections.Generic;
using CommonInterfaces.Interfaces.Models;

namespace CommonInterfaces.Interfaces.Services
{
    public interface IBackgroundJobService
    {
        bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model);

        void RemoveJobById(string jobId);

        void RemoveJobsById(List<string> jobsId);

        void RemoveAccountBackgroundJobs(IRemoveAccountJobs model);
        
        void CreateBackgroundJob(ICreateBackgroundJob model);
    }
}
