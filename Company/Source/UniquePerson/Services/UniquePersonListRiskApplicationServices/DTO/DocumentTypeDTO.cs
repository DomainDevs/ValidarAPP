using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    [DataContract]
    public class DocumentTypeDTO
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Description { get; set; }

    }
}
