using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
   public class RelationShipDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
