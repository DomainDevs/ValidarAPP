using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class PolicyPrinterClass
    {
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int PrefixNum { get; set; }
        [DataMember]
        public long DocumentNumber { get; set; }
        [DataMember]
        public int Endorsement_nro { get; set; }
    }
}
