using Sistran.Core.Application.Finances.FidelirtyServices.EEProvider.Views;
using Sistran.Core.Application.Finances.FidelityServices.EEProvider.Assemblers;
using Sistran.Core.Application.Finances.FidelityServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.Finances.FidelirtyServices.EEProvider.DAO
{
    public class FidelityDAO
    {
        public List<FidelityRisk> GetRiskFidelitiesByInsuredId(int insuredId)
        {
            List<FidelityRisk> fidelityRisks = new List<FidelityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                foreach (ISSEN.RiskFidelity entityRiskFidelity in claimRiskFidelityView.RiskFidelities)
                {
                    FidelityRisk fidelityRisk = new FidelityRisk();

                    ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                    fidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                    fidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                    fidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                    fidelityRisk.Risk.Policy.Endorsement = new Endorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    };

                    fidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    fidelityRisk.Risk.AmountInsured = insuredAmount;

                    fidelityRisks.Add(fidelityRisk);
                }

                return fidelityRisks;
            }

            return fidelityRisks;
        }

        public FidelityRisk GetRiskFidelityByRiskId(int riskId)
        {
            FidelityRisk fidelityRisk = new FidelityRisk();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                ISSEN.RiskFidelity entityRiskFidelity = claimRiskFidelityView.RiskFidelities.Cast<ISSEN.RiskFidelity>().First();
                ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                fidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                fidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                fidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                fidelityRisk.Risk.Policy.Endorsement = new Endorsement
                {
                    Id = entityEndorsementRisk.EndorsementId
                };

                fidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filterSumAssured.Equal();
                filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                builderAssured.Filter = filterSumAssured.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                decimal insuredAmount = 0;

                foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                {
                    insuredAmount += item.LimitAmount;
                }

                fidelityRisk.Risk.AmountInsured = insuredAmount;

                return fidelityRisk;
            }

            return null;
        }

        public List<FidelityRisk> GetRiskFidelitiesByEndorsementId(int endorsementId)
        {
            List<FidelityRisk> fidelityRisks = new List<FidelityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                foreach (ISSEN.RiskFidelity entityRiskFidelity in claimRiskFidelityView.RiskFidelities)
                {
                    FidelityRisk fidelityRisk = new FidelityRisk();

                    ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                    fidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                    fidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                    fidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                    fidelityRisk.Risk.Policy.Endorsement = new Endorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    };

                    fidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    fidelityRisk.Risk.AmountInsured = insuredAmount;

                    fidelityRisks.Add(fidelityRisk);
                }

                return fidelityRisks;
            }

            return fidelityRisks;
        }
    }
}
