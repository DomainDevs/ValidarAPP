using System.Runtime.Serialization;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class UtilityDetailsDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int FormUtilitys { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public int UtilitysTypeCd { get; set; }
        [DataMember]
        public int UtilitysSummaryCd { get; set; }
    
        [DataMember]
        public int UtilityId { get; set; }
        
    }
}
