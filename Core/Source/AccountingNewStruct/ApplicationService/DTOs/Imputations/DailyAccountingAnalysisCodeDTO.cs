using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// DailyAccountingAnalysisCode: Codigo De Analisis de Transacción Contabilidad Diaria
    /// </summary>
    [DataContract]
    public class DailyAccountingAnalysisCodeDTO
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
        public AnalysisCodeDTO AnalysisCode { get; set; }


        /// <summary>
        /// KeyAnalysis
        /// </summary>        
        [DataMember]
        public string KeyAnalysis { get; set; }

    }
}
