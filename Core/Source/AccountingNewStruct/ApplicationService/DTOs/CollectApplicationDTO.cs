using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Collect: Ingreso de Caja
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CollectApplicationDTO
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
        public TransactionDTO Transaction { get; set; }

        /// <summary>
        /// Aplicación
        /// </summary>        
        [DataMember]
        public ApplicationDTO Application { get; set; }
    }
}
