﻿using Constants.FunctionEnums;

namespace Services.ViewModels.GroupFunctionsModels
{
    public class FunctionViewModel
    {
        public long FunctionId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string Name { get; set; }

        public bool Assigned { get; set; }
    }
}
