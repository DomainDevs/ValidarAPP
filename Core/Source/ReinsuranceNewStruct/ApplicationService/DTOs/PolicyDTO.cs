using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PolicyDTO
    {
        /// <summary>
        /// PolicyId
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// EndorsementId
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public int DocumentNumber { get; set; }
        [DataMember]
        public int EndorsmentId { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public int BusinessType { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public EndorsementDTO Endorsement { get; set; }

        [DataMember]
        public int EffectPeriod { get; set; }
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }
        [DataMember]
        public BeneficiaryDTO Beneficiary { get; set; }
        [DataMember]
        public List<PayerComponentDTO> PayerComponents { get; set; }
        [DataMember]
        public PaymentPlanDTO PaymentPlan { get; set; }
        [DataMember]
        public string TemporalTypeDescription { get; set; }
        [DataMember]
        public List<IssuanceAgencyDTO> Agencies { get; set; }
        [DataMember]
        public HolderDTO Holder { get; set; }
        [DataMember]
        public BillingGroupDTO BillingGroup { get; set; }
        [DataMember]
        public ProductDTO Product { get; set; }
        [DataMember]
        public List<BeneficiaryDTO> DefaultBeneficiaries { get; set; }
        [DataMember]
        public PolicyTypeDTO PolicyType { get; set; }
    }
}