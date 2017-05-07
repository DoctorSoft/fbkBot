using System.Collections.Generic;
using Services.ViewModels.AccountInformationModels;
using Services.ViewModels.JobStatusModels;

namespace Services.ViewModels.HomeModels
{
    public class AccountDataViewModel
    {
        public AccountViewModel Account { get; set; }

        public AccountInformationViewModel AccountInformation { get; set; }

        public List<JobStatusViewModel> JobStatuses { get; set; }
    }
}