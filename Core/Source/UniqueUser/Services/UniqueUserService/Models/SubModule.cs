using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class SubModule : BaseSubModule
    {
        /// <summary>
        /// ModuleId
        /// </summary>
        [DataMember]
        public Module Module { get; set; }
    }
}
