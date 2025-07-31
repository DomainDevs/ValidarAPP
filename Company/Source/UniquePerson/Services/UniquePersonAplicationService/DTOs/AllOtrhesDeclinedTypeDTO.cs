using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class AllOthersDeclinedTypeDTO
    {
        [DataMember]
        public decimal Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public decimal? RoleCd { get; set; }
    }
}
