using System;

namespace Services.ViewModels.OptionsModel
{
    public class MessageViewModel
    {
        public string Text { get; set; }
        
        public TimeSpan? TimeFrom { get; set; } 
        
        public TimeSpan? TimeTo { get; set; }
        
        public bool IsEmergencyMessage { get; set; }

        public int MessageNumber { get; set; }
    }
}
