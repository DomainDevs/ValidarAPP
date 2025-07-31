using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Retentions
{
    /// <summary>
    /// RetentionConcept: Concepto de Retencion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RetentionConceptDTO
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
        public RetentionBaseDTO RetentionBase { get; set; }

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
