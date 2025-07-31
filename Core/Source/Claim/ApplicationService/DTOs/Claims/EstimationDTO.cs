using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class EstimationDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EstimationType { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int StatusCodeId { get; set; }

        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public int InternalStatusId { get; set; }

        [DataMember]
        public string InternalStatusDescription { get; set; }

        [DataMember]
        public int ReasonId { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public int CurrencyReasonId { get; set; }

        [DataMember]
        public string CurrencyReason { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal EstimateAmount { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal CoverageInsuredAmountEquivalent { get; set; }

        [DataMember]
        public decimal EstimateAmountAccumulate { get; set; }

        [DataMember]
        public int Deductible { get; set; }

        [DataMember]
        public decimal DeductibleAmount { get; set; }

        [DataMember]
        public decimal Payments { get; set; }

        [DataMember]
        public int PendingReservation { get; set; }

        [DataMember]
        public DateTime? CreationDate { get; set; }

        [DataMember]
        public decimal MinimumSalariesNumber { get; set; }

        [DataMember]
        public bool IsMinimumSalary { get; set; }

        [DataMember]
        public decimal? MinimumSalaryValue { get; set; }

    }
}
