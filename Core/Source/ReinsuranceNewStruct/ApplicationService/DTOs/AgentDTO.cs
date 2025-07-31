using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class AgentDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre completo
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        
        [DataMember]
        public string AgentType { get; set; }
        [DataMember]
        public int AgentTypeId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int IndividualType { get; set; }

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
        public string Locker { get; set; }
       
    }
}
