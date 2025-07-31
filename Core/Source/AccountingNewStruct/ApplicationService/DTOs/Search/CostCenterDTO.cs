using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CostCenterDTO
    {
        [DataMember]
        public int CostCenterId { get; set; }
		
        [DataMember]
        public string Description { get; set; }
    }
}
