using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Salvage
{
    [DataContract]
    public class SaleDTO
    {
        /// <summary>
        /// Id de la venta
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Fecha de la Venta
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Descripción de la venta
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Cantidad vendida
        /// </summary>
        [DataMember]
        public int QuantitySold { get; set; }

        /// <summary>
        /// Razón de cancelación
        /// </summary>
        [DataMember]
        public int? CancellationReasonId { get; set; }

        /// <summary>
        /// Fecha de Cancelación
        /// </summary>
        [DataMember]
        public DateTime? CancellationDate { get; set; }

        /// <summary>
        /// Total de la venta
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Identificador del comprador
        /// </summary>
        [DataMember]
        public int BuyerId { get; set; }

        /// <summary>
        /// Número completo del comprador
        /// </summary>
        [DataMember]
        public string BuyerDocumentNumber { get; set; }

        /// <summary>
        /// Nombre completo del comprador
        /// </summary>
        [DataMember]
        public string BuyerFullName { get; set; }

        /// <summary>
        /// Dirección del comprador
        /// </summary>
        [DataMember]
        public string BuyerAddress { get; set; }

        /// <summary>
        /// Teléfono del Comprador
        /// </summary>
        [DataMember]
        public string BuyerPhone { get; set; }

        /// <summary>
        /// Id de plan de pago
        /// </summary>
        [DataMember]
        public int? PaymentPlanId { get; set; }

        /// <summary>
        /// Id de moneda
        /// </summary>
        [DataMember]
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Id de clase de pago
        /// </summary>
        [DataMember]
        public int? PaymentClassId { get; set; }

        /// <summary>
        /// Impuestos
        /// </summary>
        [DataMember]
        public decimal? Tax { get; set; }

        /// <summary>
        /// Es participante
        /// </summary>
        [DataMember]
        public bool IsParticipant { get; set; }

        /// <summary>
        /// Plan de pago de la venta
        /// </summary>
        [DataMember]
        public List<PaymentQuotaDTO> PaymentQuotas { get; set; }
    }
}
