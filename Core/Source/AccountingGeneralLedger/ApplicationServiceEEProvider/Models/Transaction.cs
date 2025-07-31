using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
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
