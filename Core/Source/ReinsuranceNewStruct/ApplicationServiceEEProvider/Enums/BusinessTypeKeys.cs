using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [Flags]
    public enum BusinessTypeKeys
    {
        [EnumMember]
        [Description("100 % Compañia")]
        CompanyPercentage = 1,
        [EnumMember]
        [Description("Coaseguro aceptado")]
        Accepted = 2,
        [EnumMember]
        [Description("Coaseguro cedido")]
        Assigned = 3
    }
}

