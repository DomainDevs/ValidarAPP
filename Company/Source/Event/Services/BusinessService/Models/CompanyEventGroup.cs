using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.BusinessService.Models
{
    [DataContract]
    public class CompanyEventGroup
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int SubmoduleId { get; set; }

        [DataMember]
        public string GroupEventDescription { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public string AuthorizationReport { get; set; }

        [DataMember]
        public string ProcedureAuthorized { get; set; }

        [DataMember]
        public string ProcedureReject { get; set; }
    }
}
