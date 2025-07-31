using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.Models.Base;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Agente
    /// </summary>
    [DataContract]
    public class Agent : BaseAgent
    {
        /// <summary>
        /// Agencias
        /// </summary>
        [DataMember]
        public List<Agency> Agencies { get; set; }


        /// <summary>
        /// Agencias
        /// </summary>
        [DataMember]
        public List<AgentAgency> AgentAgencies { get; set; }


        /// <summary>
        /// Ramos
        /// </summary>
        [DataMember]
        public List<BasePrefix> Prefixes { get; set; }

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
        /// Comision del agente
        /// </summary>
        [DataMember]
        public List<CommissionAgent> ComissionAgent { get; set; }

        /// <summary>
        /// Grupo del agente
        /// </summary>
        [DataMember]
        public BaseGroupAgent GroupAgent  { get; set; }

        /// <summary>
        /// Canal del agente
        /// </summary>
        [DataMember]
        public BaseSalesChannel SalesChannel  { get; set; }

        /// <summary>
        /// Ejecutivo de cuenta del agente
        /// </summary>
        [DataMember]
        public BaseEmployeePerson EmployeePerson  { get; set; }

    }
}
