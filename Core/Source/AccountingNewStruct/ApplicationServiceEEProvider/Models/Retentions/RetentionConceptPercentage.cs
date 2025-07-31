using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions
{
    /// <summary>
    /// RetentionConceptPercentage: Procentajes del Concepto de Retencion 
    /// </summary>
    
    [DataContract]
    public class RetentionConceptPercentage
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
        public RetentionConcept RetentionConcept { get; set; }
        
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
