using CommonInterfaces.Interfaces.Models;

namespace Services.Models.Jobs
{
    public class RemoveAccountJobsModel : IRemoveAccountJobs
    {
        public string Login { get; set; }

        public long? AccountId { get; set; }

        public bool IsForSpy { get; set; }
    }
}
