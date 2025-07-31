using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Enums
{
    /// <summary>
    /// FormatTypes: Tipo de Formatos
    /// </summary>
    [DataContract]
    public enum FormatTypes
    {
        [EnumMember]
        Head = 1,
        [EnumMember]
        Detail = 2,
        [EnumMember]
        Summary = 3,


    }
}
