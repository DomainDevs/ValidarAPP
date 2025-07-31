using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class SummaryDTO
    {
        /// <summary>
        /// Obtiene o establece Id del endos0
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Total Prima
        /// </summary>
        [DataMember]
        public decimal TotalPremium { get; set; }

        /// <summary>
        /// Suma Asegurada 
        /// </summary>
        [DataMember]
        public decimal AssuredSum { get; set; }

        /// <summary>
        /// Cobertura Principal 
        /// </summary>
        [DataMember]
        public bool Primary { get; set; }


    }
}
