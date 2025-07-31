using Sistran.Core.Application.SecurityServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Models
{
    [DataContract]
    public class ModuleAcces 
    {
        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public bool Disabled { get; set; }
        
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string Image { get; set; }
        
        [DataMember]
        public bool isSearchEnabled { get; set; }
        
        [DataMember]
        public string Path { get; set; }
        
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int SubModuleId { get; set; }

        [DataMember]
        public string Module { get; set; }

        [DataMember]
        public string SubModule { get; set; }


    }
}
