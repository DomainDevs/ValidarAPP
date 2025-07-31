using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class SalePointDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
