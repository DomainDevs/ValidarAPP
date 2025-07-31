using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// CheckingAccountConcept: Concepto Cuenta Corriente
    /// </summary>
    [DataContract]
    public class CheckingAccountConceptDTO
    {

        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// ItemsEnabled
        /// </summary>
        [DataMember]
        public bool ItemsEnabled { get; set; }
    }
}
