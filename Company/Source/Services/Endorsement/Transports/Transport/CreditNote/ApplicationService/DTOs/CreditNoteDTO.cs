using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs
{
    /// <summary>
    /// modelo para endoso nota de credito
    /// </summary>
    public class CreditNoteDTO : PolicyDTO
    {
        /// <summary>
        /// fecha de vigencia desde. del endoso
        /// </summary>
        [DataMember]
        public DateTime validityDateFrom { get; set; }

        /// <summary>
        /// fecha de vigencia hasta. del endoso
        /// </summary>
        [DataMember]
        public DateTime validityDateTo { get; set; }

        /// <summary>
        /// Diferencia en días entre el inicio de la vigencia y el fin de la misma
        /// </summary>
        [DataMember]
        public int CalculateDaysBetweenDates { get; set; }

        /// <summary>
        /// texto
        /// </summary>
        [DataMember]
        public String Text { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public String Observation { get; set; }

        /// <summary>
        /// momento de la prima
        /// </summary>
        [DataMember]
        public Decimal PremiumAmount { get; set; }

        /// <summary>
        /// texto
        /// </summary>
        [DataMember]
        public String Title { get; set; }

        /// <summary>
        /// Riesgo a afectar en la nota credito
        /// </summary>
        [DataMember]
        public List<RiskDTO> Risk { get; set; }

        /// <summary>
        /// Listado de riesgos  ID y Descripcion
        /// </summary>
        [DataMember]
        public List<EndorsementTypeDTO> endorsementTypes { get; set; }

        /// <summary>
        /// listado de poliza
        /// </summary>
        [DataMember]
        public List<PolicyDTO> PolicyDTOs { get; set; }

        /// <summary>
        /// listado de riesgos por endoso
        /// </summary>
        [DataMember]
        public List<RiskDTO> RiskDTOs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int riskCoverageId { get; set; }

        /// <summary>
        /// Cobertura a afectar en la nota credito
        /// </summary>
        [DataMember]
        public List<CoverageDTO> Coverage { get; set; }

        /// <summary>
        /// Listado de DTOs de cobertura
        /// </summary>
        [DataMember]
        public List<CoverageDTO> CoverageDTOs { get; set; }

        /// <summary>
        /// Informacion calculada de la poliza
        /// </summary>
        [DataMember]
        public SummaryDTO Summary { get; set; }

        /// <summary>
        /// Id temporal de la poliza
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Valor de la prima a devolver
        /// </summary>
        [DataMember]
        public decimal? PremiumToReturn { get; set; }

        /// <summary>
        /// EndorsementController
        /// </summary>
        [DataMember]
        public string EndorsementController { get; set; }
        
        /// <summary>
        /// tipo de endoso
        /// </summary>
        [DataMember]
        public int? EndorsementType { get; set; }

        /// <summary>
        ///id  riesgo cobertura
        /// </summary>
        [DataMember]
        public int? CoverageId { get; set; }

        /// <summary>
        ///Numero de Radicado
        /// </summary>
        [DataMember]
        public int TicketNumber { get; set; }
        /// <summary>
        ///Fecha de Radicado
        /// </summary>
        [DataMember]
        public DateTime TicketDate { get; set; }
    }
}

