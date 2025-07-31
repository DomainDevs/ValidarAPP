using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
{
    /// <summary>
    /// Denuncia
    /// </summary>
    [DataContract]
    public class Claim
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime OccurrenceDate { get; set; }

        /// <summary>
        /// Id Tipo de Negocio
        /// </summary>
        [DataMember]
        public int BusinessTypeId { get; set; }

        /// <summary>
        /// Identificador del aviso
        /// </summary>
        [DataMember]
        public int? NoticeId { get; set; }

        /// <summary>
        /// Fecha de aviso
        /// </summary>
        [DataMember]
        public DateTime NoticeDate { get; set; }

        /// <summary>
        /// Descripción del riesgo
        /// </summary>
        [DataMember]
        public string RiskDescription { get; set; }

        /// <summary>
        /// Lugar de ocurrencia del siniestro
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Participación de la compañía en el pago del 
        /// </summary>
        [DataMember]
        public bool IsTotalParticipation { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [DataMember]
        public ClaimEndorsement Endorsement { get; set; }

        /// <summary>
        /// Evento catastrofico
        /// </summary>
        [DataMember]
        public CatastrophicEvent CatastrophicEvent { get; set; }

        /// <summary>
        /// Agentes de la denuncia
        /// </summary>
        [DataMember]
        public Inspection Inspection { get; set; }

        /// <summary>
        /// Lugar de ocurrencia del siniestro
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Causa del siniestro
        /// </summary>
        [DataMember]
        public Cause Cause { get; set; }

        /// <summary>
        /// Modificaciones de la denuncia
        /// </summary>
        [DataMember]
        public List<ClaimModify> Modifications { get; set; }

        /// <summary>
        /// Tipo de daño
        /// </summary>
        [DataMember]
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Responsibilidad de daño
        /// </summary>
        [DataMember]
        public DamageResponsibility DamageResponsability { get; set; }

        /// <summary>
        /// Póliticas pendiente por autorización
        /// </summary>
        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

        /// <summary>
        /// Póliticas infringidas
        /// </summary>
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        /// <summary>
        /// Tipo de riesgo cubierto
        /// </summary>
        [DataMember]
        public CoveredRiskType CoveredRiskType { get; set; }


        [DataMember]
        public TextOperation TextOperation { get; set; }
    }
}
