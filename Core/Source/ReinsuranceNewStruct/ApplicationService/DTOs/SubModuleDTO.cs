using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    public class SubModuleDTO
    {

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public string EnabledDescription { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public string VirtualFolder { get; set; }
        [DataMember]
        public int ParentModuleId { get; set; }
        [DataMember]
        public int ParentSubModuleId { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public ModuleDTO Module { get; set; }
    }
}