using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class EndoChangeText
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int policiId { get; set; }

        [DataMember]
        public int endorsementId { get; set; }

        [DataMember]
        public string textOldPolicy { get; set; }

        [DataMember]
        public string textNewPolicy { get; set; }

        [DataMember]
        public string textOldRisk { get; set; }

        [DataMember]
        public string textnewRisk { get; set; }

        [DataMember]
        public int riskId { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public string reason { get; set; }
    }
}
