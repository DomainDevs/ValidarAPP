using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.Models
{

    /// <summary>
    /// Asegurado
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.RolCompany" />
    [DataContract]
    public class Insured : BaseInsured
    {
        /// <summary>
        /// Gets or sets the insured concept.
        /// </summary>
        /// <value>
        /// The insured concept.
        /// </value>
        [DataMember]
        public InsuredConcept InsuredConcept { get; set; }

        /// <summary>
        /// Dirección de Notificación
        /// </summary>
        [DataMember]
        public CompanyName CompanyName { get; set; }

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
        public BaseInsuredSegment InsuredSegment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public BaseInsuredProfile InsuredProfile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public BaseInsuredMain InsuredMain { get; set; }
    }
}