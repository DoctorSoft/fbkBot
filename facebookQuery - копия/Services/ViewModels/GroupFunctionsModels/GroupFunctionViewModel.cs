using System.Collections.Generic;

namespace Services.ViewModels.GroupFunctionsModels
{
    public class GroupFunctionViewModel
    {
        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public List<FunctionViewModel> Functions { get; set; } 
    }
}
