using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Aviso
    /// </summary>
    [DataContract]
    public class Notice
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de Siniestro
        /// </summary>
        [DataMember]
        public DateTime ClaimDate { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Perfil del usuario
        /// </summary>
        [DataMember]
        public int UserProfileId { get; set; }

        /// <summary>
        /// Id Tipo de Riesgo Cubierto
        /// </summary>
        [DataMember]
        public int CoveredRiskTypeId { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Latitud
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Razon de Objeción
        /// </summary>
        [DataMember]
        public string ObjectedReason { get; set; }

        /// <summary>
        /// Informacion adicional
        /// </summary>
        [DataMember]
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Numero de aviso
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Numero de aviso objetado
        /// </summary>
        [DataMember]
        public int? NumberObjected { get; set; }

        /// <summary>
        /// Identificador asegurado
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Terceros afectados
        /// </summary>
        [DataMember]
        public string OthersAffected { get; set; }

        /// <summary>
        /// Monto pretendido
        /// </summary>
        [DataMember]
        public decimal? ClaimedAmount { get; set; }

        /// <summary>
        /// Identificador Temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// ClaimReasonOthers
        /// </summary>
        [DataMember]
        public string ClaimReasonOthers { get; set; }

        /// <summary>
        /// Consecutivo Interno
        /// </summary>
        [DataMember]
        public string InternalConsecutive { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        [DataMember]
        public NoticeType Type { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [DataMember]
        public ClaimEndorsement Endorsement { get; set; }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public Policy Policy { get; set; }

        [DataMember]
        public ContactInformation ContactInformation { get; set; }

        [DataMember]
        public List<NoticeCoverage> NoticeCoverages { get; set; }

        [DataMember]
        public Documentation Documentation { get; set; }

        [DataMember]
        public DamageType DamageType { get; set; }

        [DataMember]
        public DamageResponsibility DamageResponsability { get; set; }

        [DataMember]
        public NoticeReason NoticeReason { get; set; }

        [DataMember]
        public NoticeState NoticeState { get; set; }

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
    }
}
