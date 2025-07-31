using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{

    [DataContract]
    public class InsuredDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public AgencyDTO Agency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredDeclinedTypeDTO DeclinedType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredConceptDTO Concept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredProfileDTO Profile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InsuredSegmentDTO Segment { get; set; }
    }
}