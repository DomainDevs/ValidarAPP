using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{

    [DataContract]
    public class CompanyOrPerson
    {
        [DataMember]
        public int Document { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int IndividualID { get; set; }
    }
}
