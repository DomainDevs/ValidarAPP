using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class IssueAllocationDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// Facultative
        /// </summary>
        [DataMember]
        public bool Facultative { get; set; }

        /// <summary>
        /// ContractCompany
        /// </summary>
        [DataMember]
        public ContractDTO ContractCompany { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// Premium
        /// </summary>
        [DataMember]
        public AmountDTO Premium { get; set; }
    }
}
