using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Agencias
    /// </summary>
    [DataContract]
    public class CompanyAgency : BaseAgency
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public CompanyBranch Branch { get; set; }

        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public CompanyAgentDeclinedType AgentDeclinedType { get; set; }

        
        /// <summary>
        /// Agente
        /// </summary>
        [DataMember]
        public CompanyAgent Agent { get; set; }

       
        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public CompanyAgentType AgentType { get; set; }

    }
}