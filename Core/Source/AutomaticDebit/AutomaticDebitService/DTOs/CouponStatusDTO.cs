using System.Runtime.Serialization;

namespace Sistran.Core.Application.AutomaticDebitServices.DTOs
{

    [DataContract]
    public class CouponStatusDTO 
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int BankNetworkId { get; set; }

        [DataMember]
        public string BankNetworkDescription { get; set; }

        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public string InsuredDocumentNumber { get; set; }

        [DataMember]
        public string InsuredName { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public string StatusResponseId { get; set; }

        [DataMember]
        public string StatusResponse { get; set; }

        [DataMember]
        public string AuthorizationNumber { get; set; }

        [DataMember]
        public string ReceiptNumber { get; set; }

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
