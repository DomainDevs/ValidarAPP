using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
{
    [DataContract]
    public class Inspection
    {
        [DataMember]
        public int AdjusterId { get; set; }

        [DataMember]
        public int AnalizerId { get; set; }

        [DataMember]
        public int ResearcherId { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public string RegistrationHour { get; set; }

        [DataMember]
        public string AffectedProperty { get; set; }

        [DataMember]
        public string LossDescription { get; set; }
    }
}
