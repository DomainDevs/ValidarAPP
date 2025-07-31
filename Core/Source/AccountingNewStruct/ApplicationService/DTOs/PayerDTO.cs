using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Transaccion 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PayerDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// TechnicalTransaction: número de transaccion tecnica
        /// </summary>        
        [DataMember]
        public int PayerDocumentTypeId { get; set; }


        /// <summary>
        /// TechnicalTransaction: número de transaccion tecnica
        /// </summary>        
        [DataMember]
        public string PayerDocumentNumber { get; set; }

        /// <summary>
        /// TechnicalTransaction: número de transaccion tecnica
        /// </summary>        
        [DataMember]
        public string PayerName { get; set; }


        /// <summary>
        /// AccountingTransaction: número de transaccion contable
        /// </summary>        
        [DataMember]
        public int PayerTypeId { get; set; }

    }
}
