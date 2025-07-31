using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class DeductibleDTO
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
