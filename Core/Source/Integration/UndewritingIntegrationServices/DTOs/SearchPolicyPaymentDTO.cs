using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class SearchPolicyPaymentDTO
    {
        [DataMember]
        public string InsuredId { get; set; }
        [DataMember]
        public string PayerId { get; set; }
        [DataMember]
        public string AgentId { get; set; }
        [DataMember]
        public string GroupId { get; set; }
        [DataMember]
        public string PolicyId { get; set; }
        [DataMember]
        public string PolicyDocumentNumber { get; set; }
        [DataMember]
        public string SalesTicket { get; set; }
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public string PrefixId { get; set; }
        [DataMember]
        public string EndorsementId { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public string InsuredDocumentNumber { get; set; }
        [DataMember]
        public string PageSize { get; set; }
        [DataMember]
        public string PageIndex { get; set; }
        [DataMember]
        public string EndorsementDocumentNumber { get; set; }
    }
}
