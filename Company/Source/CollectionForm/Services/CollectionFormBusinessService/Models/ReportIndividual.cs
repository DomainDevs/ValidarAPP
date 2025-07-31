using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportIndividual
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
