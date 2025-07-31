using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// IssueLayer
    /// </summary>
    [DataContract]
    public class IssueAllocation

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
        public Currency Currency { get; set; }

        /// <summary>
        /// Facultative
        /// </summary>
        [DataMember]
        public bool Facultative { get; set; }

        /// <summary>
        /// ContractCompany
        /// </summary>
        [DataMember]
        public Contract ContractCompany { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// Premium
        /// </summary>
        [DataMember]
        public Amount Premium { get; set; }

        
    }
}