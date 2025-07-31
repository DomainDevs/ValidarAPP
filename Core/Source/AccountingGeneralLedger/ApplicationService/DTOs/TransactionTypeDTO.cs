using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class TransactionTypeDTO
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
        /// TotalCredit : Total de Creditos
        /// </summary>        
        [DataMember]
        public AmountDTO TotalCredit { get; set; }

        /// <summary>
        /// TotalDebit : Todal de Debitos
        /// </summary>        
        [DataMember]
        public AmountDTO TotalDebit { get; set; }
    }
}
