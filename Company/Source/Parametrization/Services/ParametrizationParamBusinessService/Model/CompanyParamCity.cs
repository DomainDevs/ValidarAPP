using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ParametrizationParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamCity
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public  CompanyParamState State { get; set; }

        [DataMember]
        public CompanyParamCountry Country { get; set; }
    }
}
