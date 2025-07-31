using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo de L�neas de contrato
    /// </summary>
    [DataContract]
    public class ContractLineDTO
    {
        /// <summary>
        /// Identificador �nico del modelo
        /// </summary>
        [DataMember]
        public int ContractLineId { get; set; }

        /// <summary>
        /// Modelo de Contrato
        /// </summary>
        [DataMember]
        public ContractDTO Contract { get; set; }

        /// <summary>
        /// Prioridad
        /// </summary>
        [DataMember]
        public int Priority { get; set; }
    }
}