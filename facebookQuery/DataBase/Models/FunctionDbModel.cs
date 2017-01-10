using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class FunctionDbModel
    {
        public long Id { get; set; }

        public FunctionName FunctionName { get; set; }

        public string Name { get; set; }

        public bool ForSpy { get; set; }

        public long FunctionTypeId { get; set; }

        public ICollection<GroupFunctionDbModel> GroupFunctions { get; set; }

        public ICollection<SpyFunctionDbModel> SpyFunctions { get; set; }

        public FunctionTypeDbModel FunctionType { get; set; }
    }
}
