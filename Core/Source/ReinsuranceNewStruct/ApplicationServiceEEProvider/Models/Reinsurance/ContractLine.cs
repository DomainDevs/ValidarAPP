#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Líneas de contrato
    /// </summary>
    [DataContract]
    public class ContractLine
    {
		/// <summary>
		/// Identificador único del modelo
		/// </summary>
		[DataMember]
		public int ContractLineId { get; set; }

		/// <summary>
		/// Modelo de Contrato
		/// </summary>
		[DataMember]
        public Contract Contract { get; set; }

        /// <summary>
        /// Prioridad
        /// </summary>
        [DataMember]
        public int Priority { get; set; }
    }
}