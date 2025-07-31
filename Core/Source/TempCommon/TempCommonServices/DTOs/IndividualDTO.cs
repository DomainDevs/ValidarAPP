using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.TempCommonServices.DTOs
{

    [DataContract]
    public class IndividualDTO 
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int IndividualTypeId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }
        
        
    }

}
