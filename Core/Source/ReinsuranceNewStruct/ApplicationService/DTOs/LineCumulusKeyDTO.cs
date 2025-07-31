using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class LineCumulusKeyDTO
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
        public LineDTO Line { get; set; }

        /// <summary>
        /// Clave de Cúmulo
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

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

        /// <summary>
        /// LineCumulusKeyRiskCoverage
        /// </summary>
        [DataMember]
        public List<LineCumulusKeyRiskCoverageDTO> LineCumulusKeyRiskCoverages { get; set; }
    }
}
