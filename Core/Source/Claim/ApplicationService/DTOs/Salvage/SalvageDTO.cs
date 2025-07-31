using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Salvage
{
    [DataContract]
    public class SalvageDTO
    {
        /// <summary>
        /// Id del salvamento
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id Sucursal de la venta
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Id Ramo de Venta
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Id del Siniestro motivo del salvamento
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

        /// <summary>
        /// Número del siniestro
        /// </summary>
        [DataMember]
        public int ClaimNumber { get; set; }

        /// <summary>
        /// Tomador de la póliza
        /// </summary>
        [DataMember]
        public int PolicyHolderId { get; set; }

        /// <summary>
        /// Descripción del Salvamento
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Asignación de Fecha
        /// </summary>
        [DataMember]
        public DateTime? AssignmentDate { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de Terminación
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ubicación
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Id Subsiniestro
        /// </summary>
        [DataMember]
        public int SubClaimId { get; set; }

        /// <summary>
        /// Valor de Estimado de la venta
        /// </summary>
        [DataMember]
        public decimal EstimatedSale { get; set; }

        /// <summary>
        /// Cantidad de Unidades
        /// </summary>
        [DataMember]
        public int UnitsQuantity { get; set; }

        /// <summary>
        /// Valor Total
        /// </summary>
        [DataMember]
        public decimal? TotalAmount { get; set; }

        [DataMember]
        public decimal RecoveryAmount { get; set; }

    }
}
