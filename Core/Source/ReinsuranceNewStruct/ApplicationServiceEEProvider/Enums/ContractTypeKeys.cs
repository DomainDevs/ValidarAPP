using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum ContractTypeKeys
    {
        [EnumMember]
        QUOTA_SHARE = 1, 
        [EnumMember]
        SURPLUS = 2, 
        [EnumMember]
        XL_EXCESS_OF_LOSS = 3, 
        [EnumMember]
        FACULTATIVO_OBLIGATORIO = 4, 
        [EnumMember]
        PROPORTIONAL_FACULTATIVE = 5, 
        [EnumMember]
        RETENCION = 6, 
        [EnumMember]
        XL_CATASTROPHIC = 7, 
        [EnumMember]
        FACULT_OBLIGATORIO_QS = 13, 
        [EnumMember]
        NON_PROPORTIONAL_FACULTATIVE = 20, 
    }
}