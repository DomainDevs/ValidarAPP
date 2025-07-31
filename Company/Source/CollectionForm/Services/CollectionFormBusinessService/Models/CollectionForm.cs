using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class CollectionForm
    {
        public CollectionForm()
        {
        }

        [DataMember]
        public ReportPolicy Policy { get; set; }

        [DataMember]
        public bool GenerateUniqueForm { get; set; }
    }
}
