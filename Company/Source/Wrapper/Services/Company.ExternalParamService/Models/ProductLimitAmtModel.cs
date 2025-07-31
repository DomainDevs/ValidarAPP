using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ProductLimitAmtModel
    {
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public decimal LimitAmt { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
