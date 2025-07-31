using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PlanFacultativeDTO
    {
        [DataMember]
        public int FacultativePaymentsId { get; set; }

        [DataMember]
        public int QuotaNumber { get; set; }

        [DataMember]
        public string DueDate { get; set; }

        [DataMember]
        public AmountDTO Amount { get; set; }
        [DataMember]
        public int TempCompanyId { get; set; }
    }
}
