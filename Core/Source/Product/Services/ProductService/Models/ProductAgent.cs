using Sistran.Core.Application.ProductServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ProductServices.Models
{
    /// <summary>
    /// agente por producto
    /// </summary>
    [DataContract]
    public class ProductAgent : BaseProductAgent
    {
        /// <summary>
        /// Id producto
        /// </summary>
        [DataMember]
        public List<ProductAgencyCommiss> ProductAgencyCommiss { get; set; }

        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public ProductAgentType AgentType { get; set; }
    }
}
