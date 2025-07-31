using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class SarlaftDTO
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
        public int IndividualId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int? BranchId { get; set; }
        [DataMember]
        public int? EconomicActivityId { get; set; }
        [DataMember]
        public int? SecondEconomicActivityId { get; set; }
        [DataMember]
        public string EconomicActivityDesc { get; set; }
        [DataMember]
        public string SecondEconomicActivityDesc { get; set; }
        [DataMember]
        public int InterviewResultId { get; set; }
        [DataMember]
        public bool PendingEvent { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public List<EventDTO> SarlaftEvent { get; set; }

        [DataMember]
        public int? YearParameter { get; set; }

        [DataMember]
        public int TypeDocument { get; set; }

        [DataMember]
        public int TypePerson { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
