using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// TransactionType:  item  imputación
    /// </summary>
    [DataContract]
    public class TransactionType
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
        public Amount TotalCredit { get; set; }

        /// <summary>
        /// TotalDebit : Todal de Debitos
        /// </summary>        
        [DataMember]
        public Amount TotalDebit { get; set; }
    }
}
