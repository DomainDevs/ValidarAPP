using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ApplicationLedgerDTO : ApplicationDTO
    {
        /// <summary>
        /// Accounting bridge Id
        /// </summary>
        [DataMember]
        public int BridgeAccountingId { get; set; }
    }
}
