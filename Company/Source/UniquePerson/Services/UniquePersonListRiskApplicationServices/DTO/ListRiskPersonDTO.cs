using System;
using System.Runtime.Serialization;
using Sistran.Company.Application.Utilities.DTO;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO
{
    [DataContract]
    public class ListRiskPersonDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DataId { get; set; }

        [DataMember]
        public DocumentTypeDTO DocumentType { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string AliasName { get; set; }

        [DataMember]
        public string BirthDate { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public DateTime AssignmentDate { get; set; }
        [DataMember]
        public Nullable<DateTime> LastChangeDate { get; set; }
        [DataMember]
        public bool ExcludedPerson { get; set; }
        [DataMember]
        public Nullable<int> UpdateListUserId { get; set; }
        [DataMember]
        public Nullable<int> CreateListUserId { get; set; }
        [DataMember]
        public string UpdateListUserName { get; set; }
        [DataMember]
        public string CreateListUserName { get; set; }

        [DataMember]
        public ListRiskDTO ListRisk { get; set; }
        [DataMember]
        public ErrorDTO Error { get; set; }
        [DataMember]
        public int Event { get; set; }
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public string Excluded { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }
    }
}
