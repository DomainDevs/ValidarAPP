using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class RiskDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
    }
}
