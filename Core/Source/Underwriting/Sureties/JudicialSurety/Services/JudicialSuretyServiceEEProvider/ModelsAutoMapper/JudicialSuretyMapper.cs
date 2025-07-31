using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper
{
    public class JudicialSuretyMapper
    {
        public ISSEN.Risk risk { get; set; }

        public ISSEN.Policy policy { get; set; }

        public ISSEN.RiskJudicialSurety RiskJudicialSurety { get; set; }

        public ISSEN.EndorsementRisk endorsementRisk { get; set; }

        public ISSEN.Endorsement endorsement { get; set; }

        public COMMEN.Article article { get; set; }
    }
}
