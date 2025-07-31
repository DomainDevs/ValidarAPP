using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
   public class PepsDTO
    {
        [DataMember]
        public int Individual_Id { get; set; }
        [DataMember]
        public Boolean? Exposed { get; set; }
        [DataMember]
        public string Trade_Name { get; set; }
        [DataMember]
        public DateTime? Unlinked_DATE { get; set; }
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
             