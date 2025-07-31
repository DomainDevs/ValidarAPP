using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using System.Collections.Generic;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs
{
    public class RiskDTO : TransportDTO
    {
        /// <summary>
        /// Endoso Id
        /// </summary>
        public int EndorsementId { get; set; }
        
        /// <summary>
        /// id del objeto del seguro
        /// </summary>
        public string InsuranceObjectId { get; set; }
  
        /// <summary>
        /// lista de riesgos por endoso
        /// </summary>
        public List<TransportDTO> Transports { get; set; }
    }
}
