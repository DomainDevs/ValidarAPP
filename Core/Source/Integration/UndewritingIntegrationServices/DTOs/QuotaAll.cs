using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    /// <summary>
    /// Cuotas Pagadores
    /// </summary>
    [DataContract]
    public class QuotaAll
    {
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public int ExchangeRate { get; set; }
        [DataMember]
        public List<FinancialPlanDTO> FinancialPlanDTOs { get; set; }
        [DataMember]
        public List<QuotaComponentDTO> QuotaComponentDTOs { get; set; }
        [DataMember]
        public List<QuotaComponentLbSbDTO> QuotaComponentLbSbDTOs { get; set; }
    }
}
