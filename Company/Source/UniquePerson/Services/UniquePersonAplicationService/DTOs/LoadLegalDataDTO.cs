using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class LoadLegalDataDTO
    {
        [DataMember]
        public List<CompanyTypeDTO> CompanyTypes { get; set; }

        [DataMember]
        public List<AssociationTypeDTO> AssociationTypes { get; set; }

        [DataMember]
        public List<DocumentTypeDTO> DocumentTypes { get; set; }
    }
}
