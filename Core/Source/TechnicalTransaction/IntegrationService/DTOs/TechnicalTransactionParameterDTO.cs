using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs
{
    [DataContract]
    public class TechnicalTransactionParameterDTO
    {
        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }
    }
}
