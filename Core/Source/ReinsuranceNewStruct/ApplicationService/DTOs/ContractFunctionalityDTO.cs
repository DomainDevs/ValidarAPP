#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo para la funcionalidad del contrato.
    /// </summary>
    [DataContract]
    public class ContractFunctionalityDTO
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int ContractFunctionalityId { get; set; }

        /// <summary>
        /// Descripción de la funcionalidad
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}