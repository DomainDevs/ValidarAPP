using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Agente Por Ramo
    /// </summary>
    [DataContract]
    public class AgentPrefix : BaseAgentPrefix
    {
        /// <summary>
        /// Obtiene o Setea el  Ramo
        /// </summary>
        /// <value>
        /// Ramo
        /// </value>
        [DataMember]
        public Prefix prefix { get; set; }
    }
}
