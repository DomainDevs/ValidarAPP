using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class BankDTO : BankBranchDTO
    {
        [DataMember]
        public List<BankBranchDTO> BankBranches { get; set; }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? BranchOffice { get; set; }

    }
}
