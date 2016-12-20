﻿using System.Collections.Generic;

namespace DataBase.Models
{
    public class SpyAccountDbModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long FacebookId { get; set; }

        public string Proxy { get; set; }

        public string ProxyLogin { get; set; }

        public string ProxyPassword { get; set; }

        public bool IsDeleted { get; set; }
        
       public CookiesForSpyDbModel Cookies { get; set; }
    }
}
