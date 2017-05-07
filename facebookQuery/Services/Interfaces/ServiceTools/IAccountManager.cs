using System.Collections.Generic;
using System.Net;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Services.ViewModels.AccountInformationModels;
using Services.ViewModels.HomeModels;

namespace Services.Interfaces.ServiceTools
{
    public interface IAccountManager
    {
        AccountViewModel GetAccountById(long accountId);

        WebProxy GetAccountProxy(AccountViewModel account);

        AccountViewModel GetAccountByFacebookId(long accountFacebookId);

        List<AccountModel> GetWorkAccounts();

        string CreateHomePageUrl(long accountFacebookId);

        AccountInformationViewModel GetAccountInformation(long accountId);

        List<AccountDataViewModel> SortAccountsByWorkStatus(List<AccountDataViewModel> accounts);
    }
}
