using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
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
