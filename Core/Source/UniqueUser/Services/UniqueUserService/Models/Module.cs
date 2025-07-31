using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class Module : BaseModule
    {
        [DataMember]
        public List<Module> SubModules { get; set; }
    }
}
