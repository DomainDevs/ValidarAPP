using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// DailyAccountingAnalysisCode: Codigo De Analisis de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingAnalysisCode
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// CostCenter 
        /// </summary>        
        [DataMember]
        public AnalysisCode AnalysisCode { get; set; }

        /// <summary>
        /// KeyAnalysis
        /// </summary>        
        [DataMember]
        public string KeyAnalysis { get; set; }
    }
}
