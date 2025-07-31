using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// FormatUsingTypes: Tipo de Usos de Formatos
    /// </summary>
    [DataContract]
    public enum FormatUsingTypes
    {
        [EnumMember]
        Sending=1,
        [EnumMember]
        Reception=2,
        [EnumMember]
        SendingNotification=3,
        [EnumMember]
        ReceptionNotification=4,
                     
    }
}
