using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class NoticeDTO
    {
        /// <summary>
        /// Identificador de aviso
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del temporal del aviso
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Fecha de aviso
        /// </summary>
        [DataMember]
        public DateTime NoticeDate { get; set; }

        /// <summary>
        /// Sucursal del aviso
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Descripción de la sucursal del aviso
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Descripción del ramo del aviso
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Descripción del ramo del aviso
        /// </summary>
        [DataMember]
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Numero de aviso
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Tipo de aviso
        /// </summary>
        [DataMember]
        public int NoticeTypeId { get; set; }

        /// <summary>
        /// Tipo de aviso
        /// </summary>
        [DataMember]
        public string NoticeTypeDescription { get; set; }

        /// <summary>
        /// Tomador de la póliza asociada
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Fecha de ocurrencia
        /// </summary>
        [DataMember]
        public DateTime OcurrenceDate { get; set; }

        /// <summary>
        /// Hora de ocurrencia
        /// </summary>
        [DataMember]
        public DateTime HourOcurrence { get; set; }

        /// <summary>
        /// Lugar de ocurrencia
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Pais de ocurrencia
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// Departamento de ocurrencia
        /// </summary>
        [DataMember]
        public int StateId { get; set; }

        /// <summary>
        /// Ciudad de ocurrencia
        /// </summary>
        [DataMember]
        public int CityId { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Identificador de la poliza
        /// </summary>
        [DataMember]
        public int? PolicyId { get; set; }

        /// <summary>
        /// Número de la poliza
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Vigencia de la póliza
        /// </summary>
        [DataMember]
        public DateTime? PolicyCurrentFrom { get; set; }

        /// <summary>
        /// Vigencia de la póliza
        /// </summary>
        [DataMember]
        public DateTime? PolicyCurrentTo { get; set; }

        [DataMember]
        public int? PolicyTypeId { get; set; }

        [DataMember]
        public int? PolicyBusinessTypeId { get; set; }

        [DataMember]
        public int? PolicyProductId { get; set; }

        /// <summary>
        /// Descripcion del riesgo
        /// </summary>
        [DataMember]
        public string RiskDescription { get; set; }

        /// <summary>
        /// Descripcion del riesgo
        /// </summary>
        [DataMember]
        public int? RiskId { get; set; }

        /// <summary>
        /// Número del riego
        /// </summary>
        [DataMember]
        public int? RiskNumber { get; set; }

        /// <summary>
        /// Identificador del tipo de riesgo cubierto
        /// </summary>
        [DataMember]
        public int? CoveredRiskTypeId { get; set; }

        /// <summary>
        /// Descripcion del aviso objetado
        /// </summary>
        [DataMember]
        public string ObjectedDescription { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public int? EndorsementNumber { get; set; }

        /// <summary>
        /// Nombre del contacto
        /// </summary>
        [DataMember]
        public string ContactName { get; set; }


        /// <summary>
        /// Telefono de contacto
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Correo del contacto
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int UserProfileId { get; set; }

        [DataMember]
        public int? DamageTypeId { get; set; }

        [DataMember]
        public int? DamageResponsibilityId { get; set; }

        [DataMember]
        public int? NoticeReasonId { get; set; }

        [DataMember]
        public string NoticeReasonDescription { get; set; }

        [DataMember]
        public int NoticeStateId { get; set; }

        [DataMember]
        public string NoticeStateDescription { get; set; }

        /// <summary>
        /// Numero de aviso objetado
        /// </summary>
        [DataMember]
        public int? NumberObjected { get; set; }

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
        /// Lista de Coberturas
        /// </summary>
        [DataMember]
        public List<NoticeCoverageDTO> Coverages { get; set; }

        /// <summary>
        /// Políticas
        /// </summary>
        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }
    }
}
