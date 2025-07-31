using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class AgentDTO
    {
        

        /// <summary>
        /// Tipo de Agente
        /// </summary>
        [DataMember]
        public int AgentTypeId { get; set; }

        /// <summary>
        /// Individual id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        

        /// <summary>
        /// Motivo de Baja
        /// </summary>
        [DataMember]
        public int AgentDeclinedTypeId { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime DateCurrent { get; set; }

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
        /// Agencias
        /// </summary>
        [DataMember]
        public List<AgencyDTO> Agencies { get; set; }


        /// <summary>
        /// Agencias
        /// </summary>
        [DataMember]
        public List<AgentAgencyDTO> AgentAgencies { get; set; }


        /// <summary>
        /// Ramos
        /// </summary>
        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; }

        /// <summary>
        /// Comisiones
        /// </summary>
        [DataMember]
        public List<ComissionAgentDTO> ComissionAgents { get; set; }
        /// <summary>
        /// Comisiones
        /// </summary>
       
        [DataMember]
        public string FullNName { get; set; }
        [DataMember]
        public string Locker { get; set; }
        [DataMember]
        public bool CommissionDiscountAgreement { get; set; }
        [DataMember]
        public int IdGroup { get; set; }
        [DataMember]
        public int IdChanel { get; set; }
        [DataMember]
        public CompanyEmployePersonDto EmployeePerson { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

    }
}
