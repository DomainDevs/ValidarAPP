#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Tipo de Contrato
    /// </summary>
    [DataContract]
    public class ContractType
    {
        /// <summary>
        /// Clave primaria del modelo
        /// </summary>
        [DataMember]
        public int ContractTypeId { get; set; }

        /// <summary>
        /// Descripción del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Estado del tipo de contrato
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Modelo de funcionalidad del contrato
        /// </summary>
        [DataMember]
        public ContractFunctionality ContractFunctionality { get; set; }
    }
}