
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// AutomaticDebitStatus: Estado Debito Automatico
    /// </summary>
    [DataContract]
    public class AutomaticDebitStatus
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

       
        
    }
}
