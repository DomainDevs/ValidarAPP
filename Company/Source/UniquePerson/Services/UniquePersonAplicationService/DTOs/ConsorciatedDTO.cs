using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ConsorciatedDTO
    {
        /// <summary>
        /// Id consorciado
        /// </summary>
        [DataMember]
        public int ConsortiumId { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
        /// <summary>
        /// Id Individuo ID
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        /// <summary>
        /// Id Asegurado 
        /// </summary>
        [DataMember]
        public int InsuredCode { get; set; }
        /// <summary>
        /// Estado 0 - Inahbailidatdo 1- Activo
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }
        /// <summary>
        /// Porcentaje de Participación
        /// </summary>
        [DataMember]
        public decimal ParticipationRate { get; set; }
        /// <summary>
        /// Fecha 
        /// </summary>
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public PersonDTO Person { get; set; }
        [DataMember]
        public CompanyDTO Company { get; set; }
        [DataMember]
        public string PersonIdentificationNumber { get; set; }
        [DataMember]
        public string CompanyIdentifationNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public int? DocumentType { get; set; }

        [DataMember]
        public ConsortiumEventDTO ConsortiumEventDTO { get; set; }
        [DataMember]
        public List<ConsortiumEventDTO> ConsortiumEventDTOs { get; set; }
    }
}
