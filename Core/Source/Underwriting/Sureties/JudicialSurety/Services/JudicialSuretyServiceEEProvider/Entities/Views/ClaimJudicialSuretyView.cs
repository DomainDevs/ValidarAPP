using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Entities.Views
{
    public class ClaimJudicialSuretyView : BusinessView
    {
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection Endorsement
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection RiskJudicialSurety
        {
            get
            {
                return this["RiskJudicialSurety"];
            }
        }

        public BusinessCollection Risks
        {
            get
            {
                return this["Risk"];
            }
        }

        public BusinessCollection RiskPayers
        {
            get
            {
                return this["RiskPayer"];
            }
        }

        public BusinessCollection RiskBeneficiaries
        {
            get
            {
                return this["RiskBeneficiary"];
            }
        }

        public BusinessCollection Articles
        {
            get
            {
                return this["Article"];
            }
        }
    }
}
