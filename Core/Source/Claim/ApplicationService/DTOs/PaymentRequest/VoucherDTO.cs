using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class VoucherDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int VoucherTypeId { get; set; }

        [DataMember]
        public string VoucherTypeDescription { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public List<VoucherConceptDTO> Concepts { get; set; }
    }
}
