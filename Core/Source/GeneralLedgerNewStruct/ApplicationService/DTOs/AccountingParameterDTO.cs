using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccountingParameterDTO
    {
        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string AccountingConceptId { get; set; }
    }
}
