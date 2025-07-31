using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger
{
    /// <summary>
    ///     Modelo que representa los Centros de Costos
    /// </summary>
    [DataContract]
    public class CostCenter
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
        public CostCenterType CostCenterType { get; set; }

        /// <summary>
        ///     Porcentaje
        /// </summary>
        [DataMember]
        public decimal PercentageAmount { get; set; }
    }
}