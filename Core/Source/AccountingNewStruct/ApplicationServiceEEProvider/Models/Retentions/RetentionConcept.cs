using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions
{
    /// <summary>
    /// RetentionConcept: Concepto de Retencion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RetentionConcept
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
       
        /// <summary>
        /// RetentionBase: Base de Reterncion 
        /// </summary>        
        [DataMember]
        public RetentionBase RetentionBase { get; set; }

        /// <summary>
        /// Status: Estado 
        /// </summary> 
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// DifferenceAmount: Cantidad tolerable de diferencia
        /// </summary>        
        [DataMember]
        public decimal DifferenceAmount { get; set; }
       
    }
}
