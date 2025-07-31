
using CLM= Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest
{
    [DataContract]
    public class PaymentRequest
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
        public DateTime? PaymentDate { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int AccountBankId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsPrinted { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public int AccountingTransaction { get; set; }

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
        public List<Voucher> Vouchers { get; set; }

        [DataMember]
        public CLM.Claim Claim { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int TemporalId { get; set; }
    }
}