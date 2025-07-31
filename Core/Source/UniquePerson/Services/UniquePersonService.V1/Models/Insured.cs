using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{

    /// <summary>
    /// Asegurado
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.RolCompany" />
    [DataContract]
    public class Insured : BaseInsured
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Agency Agency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredDeclinedType DeclinedType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredConcept Concept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredProfile Profile { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredSegment Segment { get; set; }

        [DataMember]
        public string IdentificationDocument { get; set; }

        /// <summary>
        /// Es facturador electronico?
        /// </summary>
        [DataMember]
        public bool ElectronicBiller { get; set; }

        /// <summary>
        /// Tipo de régimen
        /// </summary>
        [DataMember]
        public bool RegimeType { get; set; }


    }
}