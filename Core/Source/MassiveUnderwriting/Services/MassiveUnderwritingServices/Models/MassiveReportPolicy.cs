using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    /// <summary>
    /// Informacion de los asegurados, beneficiarios y riesgos
    /// </summary>
    [DataContract]
    public class MassiveReportPolicy
    {
        /// <summary>
        /// Fecha de Vigencia DESDE
        /// </summary>
        [DataMember]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Fecha de Vigencia HASTA	
        /// </summary>
        [DataMember]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Cód. Período de Facturación
        /// </summary>
        [DataMember]
        public int BilingCode { get; set; }

        /// <summary>
        /// Cód. Moneda
        /// </summary>
        [DataMember]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Tipo de Cambio
        /// </summary>
        [DataMember]
        public int ExchangeRate { get; set; }

        /// <summary>
        /// Cód. Grupo de Coberturas
        /// </summary>
        [DataMember]
        public int GroupCoverage { get; set; }

        /// <summary>
        /// Codigo Aliado
        /// </summary>
        [DataMember]
        public int AllianceCode { get; set; }

        /// <summary>
        /// Sucursal del Aliado
        /// </summary>
        [DataMember]
        public int AllianceBranch { get; set; }

        /// <summary>
        /// Punto de venta del aliado
        /// </summary>
        [DataMember]
        public int AllianceSalesPoint { get; set; }

        /// <summary>
        /// Número poliza externo
        /// </summary>
        [DataMember]
        public int ExternalNumberPolicy { get; set; }
    }
}
