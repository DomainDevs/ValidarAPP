using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect
{
    /// <summary>
    /// Transaccion 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class Transaction
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

       
        /// <summary>
        /// TechnicalTransaction: número de transaccion tecnica
        /// </summary>        
        [DataMember]
        public int TechnicalTransaction { get; set; }

    }
}
