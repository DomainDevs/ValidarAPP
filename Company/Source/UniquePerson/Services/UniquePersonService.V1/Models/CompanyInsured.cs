using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsured : Sistran.Core.Application.UniquePersonService.V1.Models.Base.BaseInsured
    {
        /// <summary>
        /// Gets or sets the insured concept.
        /// </summary>
        /// <value>
        /// The insured concept.
        /// </value>
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyAgency Agency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyBranch Branch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyInsuredDeclinedType DeclinedType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyInsuredConcept Concept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyInsuredProfile Profile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public CompanyInsuredSegment Segment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ElectronicBiller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool RegimeType { get; set; }


    }

}
