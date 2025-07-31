using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Retentions
{
    /// <summary>
    /// RetentionConceptPercentage: Procentajes del Concepto de Retencion 
    /// </summary>
    
    [DataContract]
    public class RetentionConceptPercentageDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// RetentionConcept: RetentionConcept
        /// </summary>
        [DataMember]
        public RetentionConceptDTO RetentionConcept { get; set; }
        
        /// <summary>
        /// Percentage: Porcentaje 
        /// </summary>        
        [DataMember]
        public decimal Percentage { get; set; }
       
        /// <summary>
        /// Date:Fecha Desde
        /// </summary>
        [DataMember]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Date:Fecha Hasta
        /// </summary>
        [DataMember]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// ExternalCode: Codigo Externo o de terceros 
        /// </summary>        
        [DataMember]
        public string ExternalCode { get; set; }
       
    }
}
