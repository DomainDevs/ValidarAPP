using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ApplicationAccountingCostCenterDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id Centro de costos para journalentry
        /// </summary>
        [DataMember]
        public int CostCenterId { get; set; }

        /// <summary>
        /// Centro de costos 
        /// </summary>
        [DataMember]
        public CostCenterDTO CostCenter { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
