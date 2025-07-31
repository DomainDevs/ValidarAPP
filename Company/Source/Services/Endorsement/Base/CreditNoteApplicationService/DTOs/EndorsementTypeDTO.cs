using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs
{
    public class EndorsementTypeDTO
    {
        public int Id { get; set; }

        public String Description { get; set; }

        public bool HasQuotation { get; set; }

        public int EndorsementId { get; set;}
        public DateTime CurrentFrom { get; set; }
        public DateTime CurrentTo { get; set; }

        public int RiskId{set; get;}
        public int CoverageId { get; set; }

        public EndorsementTypeDTO()
        {
        }
    }
}
