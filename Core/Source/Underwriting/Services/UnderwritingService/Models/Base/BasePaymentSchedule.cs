using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Planes de Pago
    /// </summary>
    [DataContract]
    public class BasePaymentSchedule : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary> 
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Iniciar Con La Fecha Mayor Entre Emisión E Inicio De Vigencia
        /// </summary> 
        [DataMember]
        public bool IsGreaterDate { get; set; }

        /// <summary>
        /// Iniciar Con La Fecha De Emisión
        /// </summary> 
        [DataMember]
        public bool IsIssueDate { get; set; }

        /// <summary>
        /// Cantidad Del Primer Pago
        /// </summary> 
        [DataMember]
        public int FirstPayerQuantity { get; set; }

        /// <summary>
        /// Cantidad De Los Pagos
        /// </summary> 
        [DataMember]
        public int Quantity { get; set; }
        /// <summary>
        /// Cantidad De Calculo De Pago
        /// </summary> 
        [DataMember]
        public int CalculationQuantity { get; set; }

        /// <summary>
        /// Cantidad Del Ultimo Pago
        /// </summary> 
        [DataMember]
        public int LastPayerQuantity { get; set; }

        /// <summary>
        /// Fecha Deshabilitado
        /// </summary> 
        [DataMember]
        public DateTime? DisabledDate { get; set; }

        /// <summary>
        /// Financiado
        /// </summary> 
        [DataMember]
        public bool PremiumFinancing { get; set; }

        /// <summary>
        /// Auto Distribución
        /// </summary> 
        [DataMember]
        public bool AutoDistribution { get; set; }

        /// <summary>
        /// Tipo De Calculo De Pago
        /// </summary> 
        [DataMember]
        public virtual PaymentCalculationType CalculationType { get; set; }
    }
}
