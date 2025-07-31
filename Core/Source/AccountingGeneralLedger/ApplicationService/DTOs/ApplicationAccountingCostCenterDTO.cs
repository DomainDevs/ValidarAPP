using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
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
