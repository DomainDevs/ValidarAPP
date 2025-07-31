using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.Adjustment.DTO
{
    [DataContract]
  public   class AdjustmentDTO: SummaryDTO
    {
        /// <summary>
        /// texto
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observation { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Días entre las vigencias
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Fecha desde del endoso
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha hasta del endoso
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public int PolicyNumber { get; set; }

        /// <summary>
        /// Póliza id
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// id endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Identificador del temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Controlador del endoso
        /// </summary>
        [DataMember]
        public string EndorsementController { get; set; }

        /// <summary>
        /// id del riesgo
        /// </summary>
        [DataMember]
        public int? RiskId { get; set; }

        /// <summary>
        /// id del grupo de cobertura del riesgo
        /// </summary>
        [DataMember]
        public int groupCoverageId { get; set; }

        /// <summary>
        /// id del objeto del seguro
        /// </summary>
        [DataMember]
        public int? InsuranceObjectId { get; set; }

        /// <summary>
        /// Endosos
        /// </summary>
        [DataMember]
        public List<EndorsementDTO> Endorsements { get; set; }

        /// <summary>
        /// Objetos del seguro
        /// </summary>
        [DataMember]
        public List<CompanyInsuredObject> InsuredObjects { get; set; }

        #region Personalización
        /// <summary>
        /// Numero de radicación
        /// </summary>
        [DataMember]
        public int TicketNumber { get; set; }

        /// <summary>
        /// Fecha de radicación
        /// </summary>
        [DataMember]
        public DateTime TicketDate { get; set; }

        #endregion
    }
}
