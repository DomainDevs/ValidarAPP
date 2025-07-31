using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs
{
    [DataContract]
    public class TechnicalTransactionDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
    }
}
