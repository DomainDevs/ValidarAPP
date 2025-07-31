using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class TransactionDTO
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
