using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.View;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Views;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Business
{
    public class BusinessAircraft
    {
        #region GetTypes
        public List<AircraftType> GetTypes(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMON.AircraftType.Properties.AircraftTypeCode, typeof(COMMON.AircraftType).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(COMMON.AircraftTypePrefix.Properties.PrefixCode, typeof(COMMON.AircraftTypePrefix).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();

            AircraftTypePrefixView view = new AircraftTypePrefixView();
            ViewBuilder builder = new ViewBuilder("TypePrefixView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AircraftTypes.Count > 0)
            {
                List<COMMON.AircraftType> entityAircraftType = view.AircraftTypes.Cast<COMMON.AircraftType>().ToList();

                List<COMMON.AircraftTypePrefix> entityAircraftTypePrefix = view.AircraftTypePrefixs.Cast<COMMON.AircraftTypePrefix>().ToList();


                return ModelAssembler.CreateAircraftType(entityAircraftType, entityAircraftTypePrefix);

            }
            else
            {
                return null;
            }

        }
        #endregion
        #region GetUses
        public List<Use> GetUses(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMON.AircraftUse.Properties.AircraftUseCode, typeof(COMMON.AircraftUse).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(COMMON.AircraftUsePrefix.Properties.PrefixCode, typeof(COMMON.AircraftUsePrefix).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();

            AircraftsUsePrefixView view = new AircraftsUsePrefixView();
            ViewBuilder builder = new ViewBuilder("UsePrefixView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AircraftUses.Count > 0)
            {
                List<COMMON.AircraftUse> entityAircraftUse = view.AircraftUses.Cast<COMMON.AircraftUse>().ToList();

                List<COMMON.AircraftUsePrefix> entityAircraftUsePrefix = view.AircraftUsePrefixs.Cast<COMMON.AircraftUsePrefix>().ToList();


                return ModelAssembler.CreateAircraftUse(entityAircraftUse, entityAircraftUsePrefix);

            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Claims

        public List<Aircraft> GetRiskAircraftsByEndorsementId(int endorsemetId)
        {
            List<Aircraft> aircrafts = new List<Aircraft>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsemetId);

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                foreach (ISSEN.RiskAircraft entityRiskAircraft in claimRiskAircraftsView.RiskAircrafts)
                {

                    ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    Aircraft aircraft = ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk, entityPolicy);
                    
                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredAirCraftView assuredView = new SumAssuredAirCraftView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredAirCraftView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    aircraft.Risk.AmountInsured = insuredAmount;

                    aircrafts.Add(aircraft);
                }

                return aircrafts;
            }

            return aircrafts;
        }

        public Aircraft GetRiskAircraftByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                ISSEN.RiskAircraft entityRiskAircraft = claimRiskAircraftsView.RiskAircrafts.Cast<ISSEN.RiskAircraft>().First();
                ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                return ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk, entityPolicy);
            }

            return null;
        }

        public List<Aircraft> GetRiskAircraftsByInsuredId(int insuredId)
        {
            List<Aircraft> aircrafts = new List<Aircraft>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);
            filter.And();
            filter.Property(ISSEN.RiskAircraft.Properties.AircraftTypeCode, typeof(ISSEN.RiskAircraft).Name);
            filter.Equal();
            filter.Constant(1);

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskAircraftsView claimRiskAircraftsView = new ClaimRiskAircraftsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskAircraftsView");
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskAircraftsView);

            if (claimRiskAircraftsView.RiskAircrafts.Count > 0)
            {
                foreach (ISSEN.RiskAircraft entityRiskAircraft in claimRiskAircraftsView.RiskAircrafts)
                {
                    ISSEN.Risk entityRisk = claimRiskAircraftsView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskAircraftsView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskAircraftsView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    Aircraft aircraft = ModelAssembler.CreateAircraft(entityRisk, entityRiskAircraft, entityEndorsementRisk, entityPolicy);
                    
                    aircrafts.Add(aircraft);
                }
            }

            return aircrafts;
        }

        #endregion
    }
}
