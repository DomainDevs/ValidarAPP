using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum TransferPaymentOrders //enum para estados de Transferencias 
    {
        [EnumMember] Canceled = 0, //0 Anulada 
        [EnumMember] Active = 1,   //1 Ingresado 
        [EnumMember] Rejected = 2  //2 Rechazado
        
    }
}
