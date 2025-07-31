using System.Runtime.Serialization;

namespace Sistran.Core.Application.WrapperAccountingService.DTOs
{
    /// <summary>
    /// Primas a Aplicar
    /// </summary>
    [DataContract]
    public class RequestApplication
    {
        /// <summary>
        /// Obtiene o setea Identificador de la Quota
        /// </summary>
        /// <value>
        /// Quota.
        /// </value>
        [DataMember]
        public int Id { get; set; }      
        [DataMember]
        public int Currency { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
