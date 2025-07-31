using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.Models
{
    public class CompanyEndorsmentCreditNote:BaseEndorsementType
    {
        public CompanyEndorsementCreditNote()
        {
            EndorsementType = new EndorsementType();
            List<EndorsementType> types = new List<EndorsementType>();
            CityFrom = new City();
            CityTo = new City();
            Text = new CompanyText();
            observaciones= new company
            Risk = new CompanyRisk();
            CompanyCoverage = new CompanyCoverage();
            premiun = new();

        }
        /// <summary>
        /// Tipo de Endoso
        /// </summary>
        public EndorsementType EndorsementType { get; set; }
        /// <summary>
        /// Desde
        /// </summary>
        public DateTime CityFrom { get; set; }
        /// <summary>
        /// Hasta
        /// </summary>
        public DateTime CityTo { get; set; }
        /// <summary>
        /// Texto
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Texto
        /// </summary>
        public string Observation{ get; set; }

        /// <summary>
        /// Riesgo
        /// </summary>
        public CompanyRisk Risk { get; set; }


    }
}
