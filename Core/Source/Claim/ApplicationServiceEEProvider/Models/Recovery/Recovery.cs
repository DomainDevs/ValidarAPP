using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery
{
    [DataContract]
    public class Recovery
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime? CancellationDate { get; set; }

        [DataMember]
        public int DocumentationId { get; set; }

        [DataMember]
        public DateTime? PrescriptionDate { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string Voucher { get; set; }

        [DataMember]
        public int RecoveryClassId { get; set; }

        [DataMember]
        public string LossResponsible { get; set; }

        [DataMember]
        public string AssignedCourt { get; set; }

        [DataMember]
        public string ExpedientNumber { get; set; }

        [DataMember]
        public DateTime? AttorneyAssingmentDate { get; set; }

        [DataMember]
        public DateTime? LastReportDate { get; set; }

        [DataMember]
        public int ClaimId { get; set; }

        [DataMember]
        public int SubClaimId { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public decimal TotalAmmount { get; set; }

        [DataMember]
        public decimal RecoveryAmount { get; set; }

        [DataMember]
        public bool DebtorIsParticipant { get; set; }

        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public Prefix Prefix { get; set; }

        [DataMember]
        public Debtor Debtor { get; set; }

        [DataMember]
        public Recuperator Recuperator { get; set; }

        [DataMember]
        public ClaimCancellationReason CancellationReason { get; set; }

        [DataMember]
        public RecoveryType RecoveryType { get; set; }

        [DataMember]
        public PaymentPlan PaymentPlan { get; set; }
    }
}
