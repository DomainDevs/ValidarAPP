using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.Enums
{
    [DataContract]
    [Flags]
    public enum EnumExchangeRateCurrency
    {
        [EnumMember]
        CURRENCY_PESOS = 0,
    }
}
