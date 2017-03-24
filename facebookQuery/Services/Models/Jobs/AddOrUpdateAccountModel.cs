using CommonInterfaces.Interfaces.Models;
using Services.ViewModels.HomeModels;

namespace Services.Models.Jobs
{
    public class AddOrUpdateAccountModel : IAddOrUpdateAccountJobs
    {
        public AccountViewModel Account { get; set; }
    }
}
