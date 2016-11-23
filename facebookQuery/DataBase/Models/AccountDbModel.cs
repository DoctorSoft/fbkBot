using System.Collections.Generic;

namespace DataBase.Models
{
    public class AccountDbModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PageUrl { get; set; }

        public long FacebookId { get; set; }

        public CookiesDbModel Cookies { get; set; }

        public ICollection<MessageDbModel> Messages { get; set; }

        public ICollection<FriendDbModel> Friends { get; set; } 
    }
}
