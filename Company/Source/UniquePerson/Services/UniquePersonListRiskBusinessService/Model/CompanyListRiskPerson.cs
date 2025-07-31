using System;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    public class CompanyListRiskPerson
    {
        public string IdCardNo { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public bool ExludedPerson { get; set; }
        public Nullable<int> UpdateListUserId { get; set; }
        public Nullable<int> CreateListUserId { get; set; }
        public int DocumentType { get; set; }
        public int ListRisk { get; set; }
        public string ListRiskDescription { get; set; }
        public bool isTemporal { get; set; }
        public int lastProcess { get; set; }
    }
}
