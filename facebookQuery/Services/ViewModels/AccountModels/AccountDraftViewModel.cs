﻿namespace Services.ViewModels.AccountModels
{
    public class AccountDraftViewModel
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long FacebookId { get; set; }

        public string Proxy { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }

        public long UserAgentId { get; set; }
    }
}
