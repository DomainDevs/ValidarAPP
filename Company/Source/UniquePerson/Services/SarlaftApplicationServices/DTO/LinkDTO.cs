using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
   public class LinkDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int LinkTypeCode { get; set; }
        [DataMember]
        public int RelationShipCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }
    }
}
