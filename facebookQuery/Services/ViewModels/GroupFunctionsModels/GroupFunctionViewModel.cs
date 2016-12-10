using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels.GroupFunctionsModels
{
    public class GroupFunctionViewModel
    {
        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public List<FunctionViewModel> Functions { get; set; } 
    }
}
