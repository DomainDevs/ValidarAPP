namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class DeductibleModelsView
    {
        /// <summary>
        /// Valor del deducible
        /// </summary>
        public decimal DeductValue { get; set; }

        /// <summary>
        /// unidades existentes
        /// </summary>
        public int DeductUnitCd { get; set; }

        /// <summary>
        /// Deducible
        /// </summary>
        public int DeductSubjectCd { get; set; }


        /// <summary>
        /// Mínimo valor del deducible
        /// </summary>
        public decimal MinDeductValue { get; set; }

        /// <summary>
        /// Mínimo unidades existentes
        /// </summary>
        public int MinDeductUnitCd { get; set; }

        /// <summary>
        /// Mínimo Deducible
        /// </summary>
        public int MinDeductSubjectCd { get; set; }


        /// <summary>
        /// Máximo valor del deducible
        /// </summary>
        public decimal? MaxDeductValue { get; set; }

        /// <summary>
        /// Máximo unidades existentes
        /// </summary>
        public int? MaxDeductUnitCd { get; set; }

        /// <summary>
        /// Máximo Deducible
        /// </summary>
        public int? MaxDeductSubjectCd { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>
        public int? RateTypeDeduct { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        public decimal? RateDeduct { get; set; }

    }
}