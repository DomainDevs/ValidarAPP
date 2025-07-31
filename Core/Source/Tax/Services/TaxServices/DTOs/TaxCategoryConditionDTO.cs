using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.DTOs
{
    [DataContract]
    public class TaxCategoryConditionDTO
    {
        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public string TaxDescription { get; set; }

        [DataMember]
        public int TaxCategoryId { get; set; }

        [DataMember]
        public string TaxCategoryDescription { get; set; }

        [DataMember]
        public int TaxConditionId { get; set; }

        [DataMember]
        public string TaxConditionDescription { get; set; }

        [DataMember]
        public decimal TaxRate { get; set; }

        [DataMember]
        public int RateTypeId { get; set; }

        [DataMember]
        public decimal TaxValue { get; set; }

        [DataMember]
        public decimal RetentionValue { get; set; }

        [DataMember]
        public bool IsRetention { get; set; }

        [DataMember]
        public decimal BaseAmount { get; set; }

        [DataMember]
        public int? RetentionTaxId { get; set; }

        [DataMember]
        public int? BaseConditionTaxId { get; set; }

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
