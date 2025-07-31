using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{

    [DataContract]
    public class AgentDTO
    {

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
        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public AgentDeclinedTypeDTO AgentDeclinedType { get; set; }

        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public AgentTypeDTO AgentType { get; set; }

        /// <summary>
        /// Grupo del agente
        /// </summary>
        //[DataMember]
        //public GroupAgentDTO GroupAgent { get; set; }

        ///// <summary>
        ///// Canal del agente
        ///// </summary>
        //[DataMember]
        //public SalesChannelDTO SalesChannel { get; set; }

        ///// <summary>
        ///// Ejecutivo de cuenta del agente
        ///// </summary>
        //[DataMember]
        //public EmployeePersonDTO EmployeePerson { get; set; }
    }
}