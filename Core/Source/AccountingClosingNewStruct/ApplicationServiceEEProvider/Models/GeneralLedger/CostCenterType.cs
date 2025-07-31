#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger
{
    /// <summary>
    ///     Modelo que representa los Tipos de Centros de Costos
    /// </summary>
    [DataContract]
    public class CostCenterType
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int CostCenterTypeId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}