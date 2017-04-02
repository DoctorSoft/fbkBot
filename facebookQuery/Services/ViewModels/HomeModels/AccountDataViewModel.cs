using Services.ViewModels.AccountInformationModels;

namespace Services.ViewModels.HomeModels
{
    public class AccountDataViewModel
    {
        public AccountViewModel Account { get; set; }

        public AccountInformationViewModel AccountInformation { get; set; }
    }
}