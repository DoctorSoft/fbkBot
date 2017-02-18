using System.Collections.Generic;

namespace DataBase.Models
{
    public class GroupSettingsDbModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<MessageDbModel> Messages { get; set; }

        public SettingsDbModel Settings { get; set; }
        
        public ICollection<AccountDbModel> Accounts { get; set; }

        public ICollection<GroupFunctionDbModel> GroupFunctions { get; set; } 
    }
}
