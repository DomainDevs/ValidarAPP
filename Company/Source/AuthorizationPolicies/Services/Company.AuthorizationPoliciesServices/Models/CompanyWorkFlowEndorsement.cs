using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class CompanyWorkFlowEndorsement
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int DocumentNumber { get; set; }
    }
}
