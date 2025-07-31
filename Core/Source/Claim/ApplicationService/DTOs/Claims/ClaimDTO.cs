using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ClaimDTO
    {
        /// <summary>
        /// Identificador de la denuncia
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

        /// <summary>
        /// Identificador del temporal de la denuncia
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Número de la denuncia
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Listado de modificaciones de la denuncia
        /// </summary>
        [DataMember]
        public List<ClaimModifyDTO> Modifications { get; set; }

        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Descripción de la sucursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Identificador del ramo
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// DEscripción del ramo
        /// </summary>
        [DataMember]
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Fecha de ocurrencia
        /// </summary>
        [DataMember]
        public DateTime OccurrenceDate { get; set; }

        /// <summary>
        /// Fecha de ocurrencia
        /// </summary>
        [DataMember]
        public DateTime? JudicialDecisionDate { get; set; }

        /// <summary>
        /// Identificador del aviso
        /// </summary>
        [DataMember]
        public int? NoticeId { get; set; }

        /// <summary>
        /// Fecha del aviso 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? NoticeDate { get; set; }

        /// <summary>
        /// Tipo de daño
        /// </summary>
        [DataMember]
        public int DamageTypeId { get; set; }

        /// <summary>
        /// Responsabilidad del daño
        /// </summary>
        [DataMember]
        public int DamageResponsabilityId { get; set; }

        /// <summary>
        /// Valor estimado
        /// </summary>
        [DataMember]
        public decimal EstimatedValue { get; set; }

        /// <summary>
        /// Valor pagado
        /// </summary>
        [DataMember]
        public decimal PaidValue { get; set; }

        /// <summary>
        /// Información de contacto
        /// </summary>
        [DataMember]
        public string ContactInformation { get; set; }

        /// <summary>
        /// Ubicación del siniestro
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Ciudad de ocurrencia del siniestro
        /// </summary>
        [DataMember]
        public int CityId { get; set; }

        /// <summary>
        /// Departamento de ocurrencia del siniestro
        /// </summary>
        [DataMember]
        public int StateId { get; set; }

        /// <summary>
        /// Pais de ocurrencia del siniestro
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// Causa del siniestro
        /// </summary>
        [DataMember]
        public int CauseId { get; set; }

        /// <summary>
        /// Descripción del siniestro
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Participación de la compañía en el siniestro
        /// </summary>
        [DataMember]
        public bool IsTotalParticipation { get; set; }

        /// <summary>
        /// Consecutivo Interno
        /// </summary>
        [DataMember]
        public string NoticeInternalConsecutive { get; set; }

        #region CatastrophicInformation

        /// <summary>
        /// Identificador de la catastrofe
        /// </summary>
        [DataMember]
        public int CatastrophicId { get; set; }

        /// <summary>
        /// Descripción de la catastrofe
        /// </summary>
        [DataMember]
        public string CatastropheDescription { get; set; }

        /// <summary>
        /// Fecha desde 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? CatastropheDateTimeFrom { get; set; }

        /// <summary>
        /// Fecha hasta 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? CatastropheDateTimeTo { get; set; }

        /// <summary>
        /// Dirección de la catastrofe 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string CatastropheAddress { get; set; }

        /// <summary>
        /// Ciudad  de la catastrofe
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int CatastropheCityId { get; set; }

        /// <summary>
        /// Departamento de la catastrofe 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int CatastropheStateId { get; set; }

        /// <summary>
        /// Pais de la catastrofe
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int CatastropheCountryId { get; set; }

        #endregion

        #region Policy

        /// <summary>
        /// Identificador de la póliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Nombre de tomador de la póliza
        /// </summary>
        [DataMember]
        public string PolicyHolderName { get; set; }

        /// <summary>
        /// Número de póliza 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        /// <summary>
        /// Identificador del endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int EndorsementNumber { get; set; }

        /// <summary>
        /// Descripcion del riesgo
        /// </summary>
        [DataMember]
        public string RiskDescription { get; set; }

        /// <summary>
        /// Tipo de riesgo cubierto
        /// </summary>
        [DataMember]
        public int CoveredRiskType { get; set; }

        /// <summary>
        /// Identificador del tomador de la póliza
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo de negocio del siniestro
        /// </summary>
        [DataMember]
        public int BusinessTypeId { get; set; }

        /// <summary>
        /// Tipo de negocio de la póliza
        /// </summary>
        [DataMember]
        public int? PolicyBusinessTypeId { get; set; }

        /// <summary>
        /// Tipo de póliza
        /// </summary>
        [DataMember]
        public int PolicyTypeId { get; set; }

        /// <summary>
        /// Producto de la póliza
        /// </summary>
        [DataMember]
        public int PolicyProductId { get; set; }

        #endregion

        #region ClaimSupplier

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int ClaimSupplierId { get; set; }

        /// <summary>
        /// Identificador del ajustador 
        /// </summary>
        [DataMember]
        public int AdjusterId { get; set; }

        /// <summary>
        /// Identificador del  
        /// </summary>
        [DataMember]
        public int ResearcherId { get; set; }

        /// <summary>
        /// Identificador del analizador 
        /// </summary>
        [DataMember]
        public int AnalizerId { get; set; }

        /// <summary>
        /// Fecha de inspección  
        /// </summary>
        [DataMember]
        public DateTime DateInspection { get; set; }

        /// <summary>
        ///  Hora de la inspección
        [DataMember]
        public string HourInspection { get; set; }
        
        /// <summary>
        /// LossDescription  
        /// </summary>
        /// <param name="LossDescription>
        /// <returns></returns>
        [DataMember]
        public string LossDescription { get; set; }

        #endregion

        /// <summary>
        /// Catastrofe
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public CatastropheDTO Catastrophe { get; set; }

        [DataMember]
        public List<PoliciesAut> AuthorizationPolicies { get; set; }

        [DataMember]
        public List<CoInsuranceAssignedDTO> coInsuranceAssigned { get; set; }
    }
}
