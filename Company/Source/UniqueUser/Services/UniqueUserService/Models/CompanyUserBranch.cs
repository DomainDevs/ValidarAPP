using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniqueUserServices.Models
{
    [DataContract]
    public class CompanyUserBranch
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Issue { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
