using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Enums
{
    public class SubBranchTypes
    {
        [Flags]
        public enum StatusItem
        {
            [EnumMember]
            Deleted = 1,
            [EnumMember]
            Modified = 2
        }
    }
}
