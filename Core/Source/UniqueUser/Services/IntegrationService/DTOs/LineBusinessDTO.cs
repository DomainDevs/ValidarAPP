using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    [DataContract]
    public class LineBusinessDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public string TyniDescription { get; set; }
        [DataMember]
        public int ReportLineBusiness { get; set; }
        [DataMember]
        public int IdLineBusinessbyRiskType { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
