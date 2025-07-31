using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class CompanyWorkFlowUser
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AccountName { get; set; }
    }
}
