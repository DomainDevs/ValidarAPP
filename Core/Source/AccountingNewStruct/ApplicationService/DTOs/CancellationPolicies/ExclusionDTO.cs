using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies
{
    /// <summary>
    /// Exclusion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class ExclusionDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Agente
        /// </summary>        
        [DataMember]
        public PersonDTO Agent { get; set; }
        
        /// <summary>
        /// Asegurado
        /// </summary>    
        [DataMember]
        public PersonDTO Insured { get; set; }
      
        /// <summary>
        /// Poliza
        /// </summary>        
        [DataMember]
        public PolicyDTO Policy { get; set; }
    }
}
