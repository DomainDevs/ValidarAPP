using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamCoverage
    {
        [DataMember]
        public int Id { get; set;}
        [DataMember]
        public string Description { get; set;}

    }
}
