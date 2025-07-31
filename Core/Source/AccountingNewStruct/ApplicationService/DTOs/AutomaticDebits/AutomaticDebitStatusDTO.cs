using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// AutomaticDebitStatus: Estado Debito Automatico
    /// </summary>
    [DataContract]
    public class AutomaticDebitStatusDTO
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
