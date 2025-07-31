using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class FacultativeCompaniesDTO
    {
        [DataMember]
        public decimal Participation { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public int Status { get; set; }

    }
}
