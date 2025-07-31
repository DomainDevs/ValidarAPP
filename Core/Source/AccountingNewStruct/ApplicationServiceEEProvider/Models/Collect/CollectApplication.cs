using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect
{
    [DataContract]
    public class CollectApplication
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
        public Collect Collect { get; set; }

        /// <summary>
        /// Transaction: Transaccion
        /// </summary>        
        [DataMember]
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Imputation: Imputación del recibo
        /// </summary>        
        [DataMember]
        public Imputations.Application Application { get; set; }
    }
}
