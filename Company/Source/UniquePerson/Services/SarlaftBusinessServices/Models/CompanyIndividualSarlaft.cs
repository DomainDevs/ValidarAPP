using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyIndividualSarlaft: BaseIndividual
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FormNum { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public DateTime? RegistrationDate { get; set; }
        [DataMember]
        public string AuthorizedBy { get; set; }
        [DataMember]
        public DateTime? FillingDate { get; set; }
        [DataMember]
        public DateTime? CheckDate { get; set; }
        [DataMember]
        public string VerifyingEmployee { get; set; }
        [DataMember]
        public DateTime? InterviewDate { get; set; }
        [DataMember]
        public string InterviewerName { get; set; }
        [DataMember]
        public bool InternationalOperations { get; set; }
        [DataMember]
        public string InterviewPlace { get; set; }
        [DataMember]
        public int? BranchId { get; set; }
        [DataMember]
        public CompanyEconomicActivity EconomicActivity { get; set; }
        [DataMember]
        public CompanyEconomicActivity SecondEconomicActivity { get; set; }
        [DataMember]
        public int InterviewResultId { get; set; }
        [DataMember]
        public bool PendingEvent { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string BranchName { get; set; }

        [DataMember]
        public int? YearParameter { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int DocumentType { get; set; }

        [DataMember]
        public int PersonType { get; set; }
    }
}
