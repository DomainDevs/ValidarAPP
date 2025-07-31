using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    public class CompanyListRiskOfac
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string EntNum { get; set; }
        [DataMember]
        public string SDNName { get; set; }

        [DataMember]
        public string AliasName { get; set; }

        [DataMember]
        public string BirthDate { get; set; }

        [DataMember]
        public string SDNType { get; set; }
        [DataMember]
        public string Program { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public string VessType { get; set; }
        [DataMember]
        public string Tonnage { get; set; }
        [DataMember]
        public string GRT { get; set; }
        [DataMember]
        public string VessFlag { get; set; }
        [DataMember]
        public string VessOwner { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public int ListRiskType { get; set; }
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public string CreatedUser { get; set; }
        [DataMember]
        public string ListRiskDescription { get; set; }
        [DataMember]
        public string ListRiskTypeDescription { get; set; }
        [DataMember]
        public int Event { get; set; }
        [DataMember]
        public int ListRiskId { get; set; }

        [DataMember]
        public DateTime AssignmentDate { get; set; }
    }
}
