using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class EventGroupDTO
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

        [DataMember]
        public string NameModule { get; set; }

        [DataMember]
        public string NameSubmodule { get; set; }
    }
}
