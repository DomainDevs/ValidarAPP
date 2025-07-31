using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsurancePaymentClaimAllocationDTO
    {

        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// EstimationType
        /// </summary>
        [DataMember]
        public EstimationTypeDTO EstimationType { get; set; }

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
        /// LevelCompanyId
        /// </summary>
        [DataMember]
        public int LevelCompanyId { get; set; }

        /// <summary>
        /// ReinsuranceSourceId
        /// </summary>
        [DataMember]
        public int ReinsuranceSourceId { get; set; }

        /// <summary>
        /// Amount 
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }
    }
}
