using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class AutomaticQuotaOperation
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? ParentId { get; set; }
        [DataMember]
        public int AutomaticOperationType { get; set; }
        [DataMember]
        public int? User { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime? ModificationDate { get; set; }
        [DataMember]
        public string Operation { get; set; }

    }
}
