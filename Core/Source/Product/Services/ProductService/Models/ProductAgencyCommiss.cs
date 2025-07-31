using Sistran.Core.Application.ProductServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ProductServices.Models
{
    /// <summary>
    /// Comision  de agencias por producto
    /// </summary>
    [DataContract]
    public class ProductAgencyCommiss : BaseProductAgencyCommiss
    {
        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public ProductAgentType AgentType { get; set; }
    }
}
