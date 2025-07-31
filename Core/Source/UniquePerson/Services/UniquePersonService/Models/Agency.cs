using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
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
        public BaseBranch Branch { get; set; }

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
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public AgentType AgentType { get; set; }

        /// <summary>
        /// Comisiones
        /// </summary>
        [DataMember]
        public List<Commission> Commissions { get; set; }
    }
}