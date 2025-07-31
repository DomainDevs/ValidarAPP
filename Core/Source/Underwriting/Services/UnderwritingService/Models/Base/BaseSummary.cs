using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.CommonService.Enums;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Resumen Póliza
    /// </summary>
    [DataContract]
    public class BaseSummary : Extension
    {
        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id póliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Id temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Id operacion
        /// </summary>
        [DataMember]
        public int OperationId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public int RiskCount { get; set; }

        /// <summary>
        /// Suma valor asegurado
        /// </summary>
        [DataMember]
        public decimal AmountInsured { get; set; }

        /// <summary>
        /// Suma Prima
        /// </summary>
        [DataMember]
        public decimal Premium { get; set; }

        /// <summary>
        /// Suma Gastos
        /// </summary>
        [DataMember]
        public decimal Expenses { get; set; }

        /// <summary>
        /// Iva
        /// </summary>
        [DataMember]
        public decimal Taxes { get; set; }
		/// <summary>
        /// Recargos
        /// </summary>
        [DataMember]
        public decimal Surcharges { get; set; }
        /// <summary>
        /// Iva
        /// </summary>
        [DataMember]
        public decimal Discounts { get; set; }
        /// <summary>
        /// Suma Prima Total
        /// </summary>
        [DataMember]
        public decimal FullPremium { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        public string ProductDescription { get; set; }

        /// <summary>
        /// Tipo de Riesgo
        /// </summary>
        [DataMember]
        public virtual CoveredRiskType? CoveredRiskType { get; set; }

        /// <summary>
        /// public decimal ExpensesLocal { get; set; }
        /// </summary>
        [DataMember]
        public decimal ExpensesLocal { get; set; }
    }
}
