using System.Runtime.Serialization;

namespace Sistran.Company.Application.ModelServices.Models
{
    [DataContract]
    public class CompanyAditionalInformationPrefix
    {
        [DataMember]
        public bool IsScore { get; set; }

        [DataMember]
        public bool IsAlliance { get; set; }

        [DataMember]
        public bool IsMassive { get; set; }

        [DataMember]
        public bool IsIssueR2 { get; set; }
        

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public int? Quote { get; set; }

        [DataMember]
        public int? Temporal { get; set; }
    }
}
