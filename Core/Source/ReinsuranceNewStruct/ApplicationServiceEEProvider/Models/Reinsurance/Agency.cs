using Sistran.Core.Application.CommonService.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Agency
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

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Participación
        /// </summary>
        [DataMember]
        public decimal Participation { get; set; }

        /// <summary>
        /// Es Principal?
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }

    }
}
