using System;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.QuotationServices.Enums
{
    public class PerilTypes
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
