using ENJU = Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Entities;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Entities
{
    public class FacadeRiskJudgement : ENJU.FacadeRiskJudgement
    {
        public int IdLastCoverage;

        /// <summary>
        /// Propiedades Públicas
        /// </summary>
        public static new class Properties
        {
            //TMP.CO_TEMP_RISK_SURETY
            public static readonly string FlatRatePercentage = "FlatRatePercentage";
            public static readonly string ServiceTypeCode = "ServiceTypeCode";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FacadeRiskJudgement()
        {

        }

        /// <summary>
        /// Tasa Única
        /// </summary>
        public decimal? FlatRatePercentage
        {
            get
            {
                return (decimal?)GetConceptByName(Properties.FlatRatePercentage);
            }
            set
            {
                SetConcept(Properties.FlatRatePercentage, value);
            }
        }

        /// <summary>
        /// Tasa Única
        /// </summary>
        public int? ServiceTypeCode
        {
            get
            {
                return (int?)GetConceptByName(Properties.ServiceTypeCode);
            }
            set
            {
                SetConcept(Properties.ServiceTypeCode, value);
            }
        }
    }
}
