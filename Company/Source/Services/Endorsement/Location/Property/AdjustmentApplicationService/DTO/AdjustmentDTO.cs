using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Location.PropertyServices.DTO;


namespace Sistran.Company.Application.AdjustmentApplicationService.DTO
{
    [DataContract]
    public class AdjustmentDTO : SummaryDTO
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
        /// Endosos
        /// </summary>
        [DataMember]
        public List<EndorsementDTO> Endorsements { get; set; }

        /// <summary>
        /// Objetos del seguro
        /// </summary>
        [DataMember]
        public List<CompanyInsuredObject> InsuredObjects { get; set; }

        /// <summary>
        /// Información del riesgo
        /// </summary>
        [DataMember]
        public List<CompanyPropertyRisk> Risks { get; set; }

        [DataMember]
        public int TicketNumber { get; set; }

        [DataMember]
        public DateTime TicketDate { get; set; }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        /// <summary>
        /// Id del Objeto del Seguro
        /// </summary>
        [DataMember]
        public int InsuredObjectId { get; set; }
    }
}
