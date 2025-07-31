using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
/// <summary>
/// 
/// </summary>
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipos de Agente
    /// </summary>
    [DataContract]
    public class AgentType : Extension
    {
        /// <summary>        
        /// Identificador Tipo Agente
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>        
        /// Descripcion Tipo Agente
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>        
        /// Minima Descripcion Tipo Agente
        /// </value>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
