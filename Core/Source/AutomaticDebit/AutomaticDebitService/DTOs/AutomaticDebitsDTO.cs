using System.Runtime.Serialization;

namespace Sistran.Core.Application.AutomaticDebitServices.DTOs
{

    [DataContract]
    public class AutomaticDebitsDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int BankNetworkId { get; set; }

        [DataMember]
        public string BankNetworkDescription { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public string ProcessDate { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string StatusDescription { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserNick { get; set; }

        [DataMember]
        public int Rows { get; set; }
    }

}
