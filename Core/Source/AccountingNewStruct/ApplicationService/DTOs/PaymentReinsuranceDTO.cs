using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// ReinsurancePayment 
    /// </summary>
    [DataContract]
    public class PaymentReinsuranceDTO
    {      
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }
      
        /// <summary>
        /// PaymentNumber
        /// </summary>
        [DataMember]
        public int PaymentNumber { get; set; }

        /// <summary>
        /// PaymentDate
        /// </summary>
        [DataMember]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Reinsurance Movements 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Movements { get; set; }

        /// <summary>
        /// MovementType 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public MovementTypeDTO MovementType { get; set; }

        /// <summary>
        /// RiskNumber
        /// </summary>
        [DataMember]
        public int RiskNumber { get; set; }

        /// <summary>
        /// CoverageNumber
        /// </summary>
        [DataMember]
        public int CoverageNumber { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// ClaimId
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

    }
}
