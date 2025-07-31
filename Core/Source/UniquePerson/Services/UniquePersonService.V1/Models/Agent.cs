using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Agente
    /// </summary>
    [DataContract]
    public class Agent : BaseAgent
    {
        
        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public AgentDeclinedType AgentDeclinedType { get; set; }

        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public AgentType AgentType { get; set; }
        
        /// <summary>
        /// Grupo del agente
        /// </summary>
        [DataMember]
        public GroupAgent GroupAgent { get; set; }

        /// <summary>
        /// Canal del agente
        /// </summary>
        [DataMember]
        public SalesChannel SalesChannel { get; set; }

        /// <summary>
        /// Ejecutivo de cuenta del agente
        /// </summary>
        [DataMember]
        public EmployeePerson EmployeePerson { get; set; }

    }
}
