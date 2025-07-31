using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.FinancialPlanServices.DTOs
{
    /// <summary>
    /// Filtro recuotificacion
    /// </summary>
    [DataContract]
    public class FilterFinancialPlanDTO
    {
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PaymentPlanId { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public DateTime AccountDate { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public bool IsQuota { get; set; }

        [DataMember]
        public string ReasonforChange { get; set; }
    }
}
