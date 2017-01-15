﻿using Constants.FunctionEnums;

namespace Services.ViewModels.SpySettingsViewModels
{
    public class SpyFunctionViewModel
    {
        public long FunctionId { get; set; }

        public FunctionName FunctionName { get; set; }

        public string Name { get; set; }

        public FunctionTypeName FunctionTypeName { get; set; }

        public string TypeName { get; set; }

        public bool Assigned { get; set; }
    }
}