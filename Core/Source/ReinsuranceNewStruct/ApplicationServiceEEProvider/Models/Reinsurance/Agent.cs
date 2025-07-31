using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Agente
     /// </summary>
    [DataContract]
    public class Agent 
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

        /// <summary>
        /// Individual id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime DateCurrent { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }

        /// <summary>
        /// Fecha de Modificación
        /// </summary>
        [DataMember]
        public DateTime? DateModification { get; set; }



        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Locker { get; set; }

    }
}
