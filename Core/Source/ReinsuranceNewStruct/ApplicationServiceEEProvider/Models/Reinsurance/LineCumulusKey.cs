using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// LineCumulusKey
    /// </summary>
    [DataContract]
    public class LineCumulusKey
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]    
        public int Id { get; set; }

        /// <summary>
        /// Line
        /// </summary>
        [DataMember]
        public Line Line { get; set; }

        /// <summary>
        /// Clave de Cúmulo
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

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

        /// <summary>
        /// LineCumulusKeyRiskCoverage
        /// </summary>
        [DataMember]
        public List<LineCumulusKeyRiskCoverage> LineCumulusKeyRiskCoverages { get; set; }
    }
}