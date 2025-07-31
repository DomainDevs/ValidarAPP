using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models.Formats
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
        Detail=2,
        [EnumMember]
        Summary=3,

                
    }
}
