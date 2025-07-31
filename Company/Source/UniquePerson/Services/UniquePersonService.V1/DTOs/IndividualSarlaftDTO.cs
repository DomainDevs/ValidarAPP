using Sistran.Company.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.DTOs
{
    [DataContract]
    public class IndividualSarlaftDTO : ParametricServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public DateTime? RegistrationDate { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string VerifyingEmployee { get; set; }
        [DataMember]
        public bool PendingEvents { get; set; }
        [DataMember]
        public int InterviewResultCode { get; set; }
        [DataMember]
        public string InterviewerPlace { get; set; }
        [DataMember]
        public string InterviewerName { get; set; }
        [DataMember]
        public bool InternationalOperations { get; set; }
        [DataMember]
        public string FormNum { get; set; }
        [DataMember]
        public int ActivityEconomic { get; set; }
        [DataMember]
        public string AuthorizedBy { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public SarlaftDTO finacialSarlaft { get; set; }
    }
}
