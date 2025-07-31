using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class RegularizedPaymentDTO 
    {
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int PaymentStatus { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int LogStatus { get; set; }
        [DataMember]
        public int RegularizePaymentCode { get; set; }
        [DataMember]
        public int SourceCode { get; set; }

        
    }
}
