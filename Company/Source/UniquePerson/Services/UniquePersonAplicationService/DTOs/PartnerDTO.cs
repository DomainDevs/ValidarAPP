using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class PartnerDTO
    {
        
        [DataMember]
        public bool Active{ get; set; }
        
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int PartnerId{ get; set; }

        [DataMember]
        public string TradeName{ get; set; }

        [DataMember]
        public StatusTypeService statusTypeService { get; set; }

        [DataMember]
        public string IdentificationDocumentNumber { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string Description { get; set; }
       

    }
}
