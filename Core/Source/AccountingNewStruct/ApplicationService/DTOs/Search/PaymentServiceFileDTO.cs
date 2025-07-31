using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentServiceFileDTO
    {
        [DataMember]
        public int TempPaymentServiceFileCode { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public string FileName { get; set; }
    }
}
