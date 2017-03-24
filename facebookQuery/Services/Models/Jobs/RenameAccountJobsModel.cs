using CommonInterfaces.Interfaces.Models;
using Services.ViewModels.HomeModels;

namespace Services.Models.Jobs
{
    public class RenameAccountJobsModel : IRenameAccountJobs
    {
        public AccountViewModel AccountViewModel { get; set; }

        public string OldLogin { get; set; }
    }
}
