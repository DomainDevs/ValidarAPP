#region Using
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo para la funcionalidad del contrato.
    /// </summary>
    [DataContract]
    public class ContractFunctionality
    {
        /// <summary>
        /// Identificador �nico del modelo
        /// </summary>
        [DataMember]
        public int ContractFunctionalityId { get; set; }

        /// <summary>
        /// Descripci�n de la funcionalidad
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}