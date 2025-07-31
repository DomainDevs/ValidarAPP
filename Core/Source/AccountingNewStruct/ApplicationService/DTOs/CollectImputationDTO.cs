using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Collect: Ingreso de Caja
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CollectImputationDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Collect: Ingreso de Caja
        /// </summary>        
        [DataMember]
        public CollectDTO Collect { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public TransactionDTO Transaction { get;set;}

        /// <summary>
        /// Imputation: Imputación del recibo
        /// </summary>        
        [DataMember]
        public ImputationDTO Imputation { get; set; }

    }
}
