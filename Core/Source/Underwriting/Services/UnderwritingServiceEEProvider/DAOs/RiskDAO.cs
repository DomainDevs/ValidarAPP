using System;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskDAO
    {
        public List<Models.Risk> GetRisksByPolicyIdEndorsmentId(int policyId, int endorsementId)
        {

            List<Models.Risk> risks = new List<Models.Risk>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            EndorsementRiskView view = new EndorsementRiskView();
            ViewBuilder builder = new ViewBuilder("EndorsementRiskView");
            builder.Filter = filter.GetPredicate();
            
            var daf = DataFacadeManager.Instance.GetDataFacade();
            daf.FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                foreach (ISSEN.Risk entityRisk in view.Risks)
                {
                    Models.Risk risk = new Models.Risk();
                    risk.RiskId = entityRisk.RiskId;
                    risks.Add(risk);
                }

                return risks;
            }
            else
            {
                return risks;
            }
            
        }

        public Models.Risk GetRiskSuretyByRiskId(int riskId)
        {
            Models.Risk risk = new Models.Risk();
            PrimaryKey primaryKey = ISSEN.RiskSurety.CreatePrimaryKey(riskId);
            ISSEN.RiskSurety entityRiskSurety = (ISSEN.RiskSurety)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityRiskSurety != null)
            {
                risk.RiskId = entityRiskSurety.RiskId;
                risk.SecondInsured = new IssuanceInsured
                {
                    IndividualId = entityRiskSurety.IndividualId
                };
                
                return risk;
            }
            else
            {
                return risk;
            }
        }
    }
}
