using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo que representa los Centros de Costos
    /// </summary>
    [DataContract]
    public class CostCenterDTO
    {
        /// <summary>
        ///     Codigo
        /// </summary>
        [DataMember]
        public int CostCenterId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Prorrateado
        /// </summary>
        [DataMember]
        public bool IsProrated { get; set; }

        /// <summary>
        ///     Tipo
        /// </summary>
        [DataMember]
        public CostCenterTypeDTO CostCenterType { get; set; }

        /// <summary>
        ///     Porcentaje
        /// </summary>
        [DataMember]
        public decimal PercentageAmount { get; set; }
    }
}