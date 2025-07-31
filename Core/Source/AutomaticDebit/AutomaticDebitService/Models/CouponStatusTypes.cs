using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// CouponStatusTypes: Tipo del Estado del Debito 
    /// </summary>
    [DataContract]
    public enum CouponStatusTypes
    {
        [EnumMember]
        Rejected=1,
        [EnumMember]
        Applied=2,

                
    }
}
