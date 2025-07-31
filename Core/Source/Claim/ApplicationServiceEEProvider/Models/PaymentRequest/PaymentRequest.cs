
using CLM = Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
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
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string IndividualName { get; set; }

        [DataMember]
        public string IndividualDocumentNumber { get; set; }

        [DataMember]
        public int IndividualDocumentTypeId { get; set; }

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
        public int SalePointId { get; set; }

        [DataMember]
        public bool IsTotal { get; set; }

        [DataMember]
        public string SaveDailyEntryMessage { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

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

        //[DataMember]
        //public List<Voucher> Vouchers { get; set; }

        //[DataMember]
        //public CLM.Claim Claim { get; set; }

        [DataMember]
        public List<CLM.Claim> Claims { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int TemporalId { get; set; }
        
        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public List<PaymentRequestCoInsurance> CoInsurance { get; set; }

        [DataMember]
        public AccountingPaymentRequest AccountingPayment { get; set; }

    }
}