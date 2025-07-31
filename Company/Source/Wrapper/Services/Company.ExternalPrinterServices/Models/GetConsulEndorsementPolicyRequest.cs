using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class GetConsulEndorsementPolicyRequest
    {
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int PrefixNum { get; set; }
        [DataMember]
        public long DocumentNumber { get; set; }
        [DataMember]
        public int Typequery { get; set; }
    }
}
