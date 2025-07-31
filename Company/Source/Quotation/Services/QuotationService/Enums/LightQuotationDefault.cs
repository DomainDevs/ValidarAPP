using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.QuotationServices.Enums
{
    public class LightQuotationDefault
    {
        [Flags]
        public enum BranchDefault
        {
            [EnumMember]
            LightQuotation = 91
           
        }
}
}
