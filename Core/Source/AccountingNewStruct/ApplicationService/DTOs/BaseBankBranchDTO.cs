using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class BankBranchDTO
    {
        [DataMember]
        public BankDTO Bank { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
