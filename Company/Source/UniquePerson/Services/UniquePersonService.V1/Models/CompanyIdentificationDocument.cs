using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{         
[DataContract]
    public class CompanyIdentificationDocument : BaseIdentificationDocument
    {
        [DataMember]
        public CompanyDocumentType DocumentType { get; set; }

        public string NitAssociationType { get; set; }
    }
}
