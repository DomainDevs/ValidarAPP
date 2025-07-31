using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models.Formats
{
    /// <summary>
    /// FileTypes: Tipo de Archivos
    /// </summary>
    [DataContract]
    public enum FileTypes
    {
        [EnumMember]
        Text = 1,
        [EnumMember]
        Excel=2,
        [EnumMember]
        ExcelTemplate=3                
    }
}
