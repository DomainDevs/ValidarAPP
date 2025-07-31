using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class PaymentRequestCoInsurance
    {
        [DataMember]
        public int PaymentRequestId { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int TypePaymentRequestId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal PartCiaPct { get; set; }

        [DataMember]
        public int UserId { get; set; }

    }
}
