using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Agencias
    /// </summary>
    [DataContract]
    public class Agency : BaseAgency
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public AgentDeclinedType AgentDeclinedType { get; set; }

        
        /// <summary>
        /// Agente
        /// </summary>
        [DataMember]
        public Agent Agent { get; set; }

       
        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public AgentType AgentType { get; set; }

    }
}