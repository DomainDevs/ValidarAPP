using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Declaration.DTO
{
    [DataContract]
    public class EndorsementDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal PolicyNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IdEndorsement { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public EndorsementType? EndorsementType { get; set; }
    }
}
