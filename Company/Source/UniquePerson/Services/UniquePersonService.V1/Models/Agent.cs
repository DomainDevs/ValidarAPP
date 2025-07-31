using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Agente
    /// </summary>
    [DataContract]
    public class CompanyAgent : BaseAgent
    {
        
        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public CompanyAgentDeclinedType AgentDeclinedType { get; set; }

        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public CompanyAgentType AgentType { get; set; }
        
        /// <summary>
        /// Grupo del agente
        /// </summary>
        [DataMember]
        public CompanyGroupAgent GroupAgent { get; set; }

        /// <summary>
        /// Canal del agente
        /// </summary>
        [DataMember]
        public CompanySalesChannel SalesChannel { get; set; }

        /// <summary>
        /// Ejecutivo de cuenta del agente
        /// </summary>
        [DataMember]
        public CompanyEmployeePerson EmployeePerson { get; set; }

    }
}
