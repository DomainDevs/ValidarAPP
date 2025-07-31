using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssuanceEntities = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper
{
    public class CompanyJudgementMapper
    {
        public IssuanceEntities.Risk risk { get; set; }
        public IssuanceEntities.RiskJudicialSurety RiskJudicialSurety { get; set; }
        public IssuanceEntities.EndorsementRisk endorsementRisk { get; set; }
        public IssuanceEntities.Endorsement endorsement { get; set; }
    }
}
