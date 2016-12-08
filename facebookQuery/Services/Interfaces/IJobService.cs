﻿using Services.ViewModels.HomeModels;

namespace Services.Interfaces
{
    public interface IJobService
    {
        void AddOrUpdateAccountJobs(AccountViewModel accountViewModel);

        void RemoveAccountJobs(string login);

        void RenameAccountJobs(AccountViewModel accountViewModel, string oldLogin);
    }
}