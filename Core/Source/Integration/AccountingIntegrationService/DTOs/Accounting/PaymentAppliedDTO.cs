using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PaymentAppliedDTO
    {
        [DataMember]
        public int Id { get; set; }      
        [DataMember]
        public List<PaymentQuotaDTO> Quotas { get; set; }
    }
}
