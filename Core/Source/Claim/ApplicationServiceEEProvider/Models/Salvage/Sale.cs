using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage
{
    [DataContract]
    public class Sale
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public DateTime? CancellationDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public int SoldQuantity { get; set; }

        [DataMember]
        public bool IsParticipant { get; set; }

        [DataMember]
        public ClaimCancellationReason CancellationReason { get; set; }

        [DataMember]
        public Buyer Buyer { get; set; }

        [DataMember]
        public PaymentPlan PaymentPlan { get; set; }

    }
}
