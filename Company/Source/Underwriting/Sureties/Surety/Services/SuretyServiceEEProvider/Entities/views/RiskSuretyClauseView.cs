using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskSuretyClauseView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskClauses
        {
            get
            {
                return this["RiskClause"];
            }
        }

    }
}
