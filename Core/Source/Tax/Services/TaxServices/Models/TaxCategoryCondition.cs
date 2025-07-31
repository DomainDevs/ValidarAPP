using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.TaxServices.Enums;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.TaxServices.Models
{

    /// <summary>
    /// Impuestos
    /// </summary>
    [DataContract]
    public class TaxCategoryCondition : Extension
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
        /// ¿Es Retensión?
        /// </summary>
        [DataMember]
        public bool IsRetention { get; set; }

        /// <summary>
        /// ¿Está habilitado?
        /// </summary>
        [DataMember]
        public bool IsEnable { get; set; }

        /// <summary>
        /// Tipo de tasa
        /// </summary>
        [DataMember]
        public RateType RateType { get; set; }

        /// <summary>
        /// Valor base
        /// </summary>
        [DataMember]
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// Valor del impuesto
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Impuesto de retención
        /// </summary>
        [DataMember]
        public int? RetentionTaxId { get; set; }

        /// <summary>
        /// Impuesto base para la retensión
        /// </summary>
        [DataMember]
        public int? BaseConditionTaxId { get; set; }

        /// <summary>
        /// Condición
        /// </summary>
        [DataMember]
        public TaxCondition TaxCondition { get; set; }

        /// <summary>
        /// Categoría
        /// </summary>
        [DataMember]
        public TaxCategory TaxCategory { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public TaxPeriodRate TaxPeriodRate { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        ///concepto de contabilidad
        /// </summary>
        [DataMember]
        public int AccountingConceptId { get; set; }

        /// <summary>
        /// Actividad economica para el impuesto
        /// </summary>
        [DataMember]
        public int EconomicActivityTaxId { get; set; }
    }

}
