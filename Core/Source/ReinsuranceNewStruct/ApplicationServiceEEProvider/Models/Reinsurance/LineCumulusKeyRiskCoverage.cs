#region Using

using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// LineCumulusKeyRiskCoverage
    /// </summary>
    [DataContract]
    public class LineCumulusKeyRiskCoverage
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]    
        public int Id { get; set; }

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

        
    }
}