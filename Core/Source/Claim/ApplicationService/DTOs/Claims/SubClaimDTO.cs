using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class SubClaimDTO
    {
        [DataMember]
        public int ClaimId { get; set; }

        [DataMember]
        public int? NoticeId { get; set; }

        [DataMember]
        public DateTime NoticeDate { get; set; }

        [DataMember]
        public int CauseId { get; set; }

        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public int? CountryId { get; set; }

        [DataMember]
        public int? StateId { get; set; }

        [DataMember]
        public int? CityId { get; set; }

        [DataMember]
        public DateTime OccurrenceDate { get; set; }

        [DataMember]
        public DateTime? JudicialDecisionDate { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public int ClaimModifyId { get; set; }

        [DataMember]
        public int ModificationNumber { get; set; }

        [DataMember]
        public int SubClaim { get; set; }

        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public string RiskDescription { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public string CoverageDescription { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }

        [DataMember]
        public bool IsInsured { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public bool IsProspect { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public string EstimationType { get; set; }

        [DataMember]
        public decimal PaymentValue { get; set; }

        [DataMember]
        public decimal EstimateAmount { get; set; }

        [DataMember]
        public decimal EstimateAmountAccumulate { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal TotalConcept { get; set; }

        [DataMember]
        public decimal TotalTax { get; set; }

        [DataMember]
        public decimal TotalRetention { get; set; }

        [DataMember]
        public decimal DeductibleAmount { get; set; }

        [DataMember]
        public decimal DeductibleNet { get; set; }

        [DataMember]
        public int EstimationTypeEstatus { get; set; }

        [DataMember]
        public int EstimationTypeInternalStatusId { get; set; }

        [DataMember]
        public int EstimationTypeEstatusReasonCode { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public DateTime EstimationDate { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int? ClaimCoverageId { get; set; }

        [DataMember]
        public int? DamageTypeCode { get; set; }

        [DataMember]
        public int? DamageResponsibilityCode { get; set; }

        [DataMember]
        public decimal? InsuredAmountTotal { get; set; }

        [DataMember]
        public string EstimationTypeStatusReasonDescription { get; set; }

        [DataMember]
        public string EstimationTypeStatusDescription { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public bool IsTotalParticipation { get; set; }

        [DataMember]
        public decimal ClaimedAmount { get; set; }

        [DataMember]
        public bool IsClaimedAmount { get; set; }

        /// <summary>
        /// Nombre de Afectado
        /// </summary>
        [DataMember]
        public string AffectedFullName { get; set; }
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int PolicyBusinessTypeId { get; set; }

        [DataMember]
        public decimal MinimumSalariesNumber { get; set; }

        [DataMember]
        public bool IsMinimumSalary { get; set; }

        [DataMember]
        public decimal Reservation { get; set; }

        [DataMember]
        public decimal? MinimumSalaryValue { get; set; }

        [DataMember]
        public string TypeAmountDescription { get; set; }

        [DataMember]
        public string PolicyHolderName { get; set; }

        [DataMember]
        public string PolicySalePoint { get; set; }

        [DataMember]
        public int EstimationTypeStatusReasonId  { get; set; }

        [DataMember]
        public int EstimationTypeStatusId { get; set; }

        [DataMember]
        public int EstimationCurrencyId { get; set; }

        [DataMember]
        public int PolicyProductId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Recibo y conceptos de pago para solicitud de pago
        /// </summary>
        [DataMember]
        public List<VoucherDTO> Vouchers { get; set; }
    }    
}
