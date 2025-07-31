using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
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
        public int CostCenterTypeId { get; set; }

        /// <summary>
        ///     Porcentaje
        /// </summary>
        [DataMember]
        public decimal PercentageAmount { get; set; }
    }
}
