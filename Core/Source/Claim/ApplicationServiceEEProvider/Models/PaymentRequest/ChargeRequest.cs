using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using CLM=Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class ChargeRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime EstimatedDate { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int AccountBankId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsPrinted { get; set; }

        [DataMember]
        public decimal RecoveryOrSalvageAmount { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int SalvageId { get; set; }

        [DataMember]
        public int RecoveryId { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public Prefix Prefix { get; set; }

        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public MovementType MovementType { get; set; }

        [DataMember]
        public ClaimPersonType PersonType { get; set; }

        [DataMember]
        public Currency Currency { get; set; }

        [DataMember]
        public PaymentRequestType Type { get; set; }

        [DataMember]
        public ClaimPaymentMethod PaymentMethod { get; set; }

        [DataMember]
        public Voucher Voucher { get; set; }

        [DataMember]
        public CLM.Claim Claim { get; set; }

        [DataMember]
        public Participant Participant { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }                

        [DataMember]
        public int TemporalId { get; set; }

        [DataMember]
        public AccountingPaymentRequest AccountingCharge { get; set; }
    }
}
