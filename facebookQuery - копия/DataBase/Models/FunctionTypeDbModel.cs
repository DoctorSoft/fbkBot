using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class FunctionTypeDbModel
    {
        public long Id { get; set; }

        public FunctionTypeName FunctionTypeName { get; set; }

        public string TypeName { get; set; }

        public ICollection<FunctionDbModel> Functions { get; set; } 
    }
}
