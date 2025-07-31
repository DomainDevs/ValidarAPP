using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class EndorsementCompanyDTO
    {
        /// <summary>
        /// Obtiene o establece Id del endos0
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion estado Endoso
        /// </summary>
        [DataMember]
        public string DescriptionProduct { get; set; }

        /// <summary>
        /// Numero de endoso
        /// </summary>
        [DataMember]
        public int EndorsementNumber { get; set; }

        /// <summary>
        /// Tipo de endoso
        /// </summary>
        /// <value>
        /// The type of the endorsement.
        /// </value>
        [DataMember]
        public int EndorsementType { get; set; }

        /// <summary>
        /// Descripcion tipo de endoso
        /// </summary>
        /// <value>
        /// The type of the endorsement.
        /// </value>
        [DataMember]
        public string DescriptionEndorsementType { get; set; }

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
        /// Fecha de emision
        /// </summary>
        [DataMember]
        public DateTime EmissionDate { get; set; }

        
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public string ConditionText { get; set; }

        [DataMember]
        public string Annotations { get; set; }

        [DataMember]
        public int ModificationTypeId { get; set; }

    }
}
