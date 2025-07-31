using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    /// <summary>
    /// Comprobante
    /// </summary>
    [DataContract]
    public class Voucher 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public int PaymentRequestId { get; set; }

        [DataMember]
        public VoucherType VoucherType { get; set; }

        [DataMember]
        public Currency Currency { get; set; }

        [DataMember]
        public EstimationType EstimationType { get; set; }

        [DataMember]
        public List<VoucherConcept> Concepts { get; set; }
    }
}
