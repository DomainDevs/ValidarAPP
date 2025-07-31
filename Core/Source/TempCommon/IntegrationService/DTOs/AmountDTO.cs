using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.TempCommonService.DTOs
{
    [DataContract]
    public class AmountDTO
    {
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
    }
}
