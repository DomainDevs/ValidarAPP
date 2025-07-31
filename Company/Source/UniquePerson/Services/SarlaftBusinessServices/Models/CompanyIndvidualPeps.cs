using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyIndvidualPeps
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public Boolean? Exposed { get; set; }
        [DataMember]
        public string TradeName { get; set; }
        [DataMember]
        public DateTime? UnlinkedDATE { get; set; }
        [DataMember]
        public int? Category { get; set; }
        [DataMember]
        public int? Link { get; set; }
        [DataMember]
        public int? Affinity { get; set; }
        [DataMember]
        public int? Unlinked { get; set; }

        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public string Observations { get; set; }
        [DataMember]
        public string JobOffice { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }

    }
}
