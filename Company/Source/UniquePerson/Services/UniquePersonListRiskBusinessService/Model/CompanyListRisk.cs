using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    [DataContract]
    public class CompanyListRisk
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? DocumentType { get; set; }

        [DataMember]
        public string DocumentTypeDescription { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Alias { get; set; }

        [DataMember]
        public string BirthDate { get; set; }

        [DataMember]
        public int ListRiskId { get; set; }

        [DataMember]
        public string ListRiskDescription { get; set; }

        [DataMember]
        public int ListRiskType { get; set; }

        [DataMember]
        public string ListRiskTypeDescription { get; set; }

        [DataMember]
        public string CreatedUser { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int Event { get; set; }

        [DataMember]
        public DateTime AssignmentDate { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }
    }
}
