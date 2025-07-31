using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using System.Runtime.Serialization;

//SISTRAN



namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies
{
    /// <summary>
    /// Exclusion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class Exclusion
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
        public UniquePersonService.V1.Models.Person Agent { get; set; }
        
        /// <summary>
        /// Asegurado
        /// </summary>    
        [DataMember]
        public UniquePersonService.V1.Models.Person Insured { get; set; }
      

        /// <summary>
        /// Poliza
        /// </summary>        
        [DataMember]
        public Policy Policy { get; set; }
    }
}
