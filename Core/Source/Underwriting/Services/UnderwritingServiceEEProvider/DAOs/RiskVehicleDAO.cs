using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskVehicleDAO
    {
        public List<Model.RiskVehicle> GetRisksByPlate(string description)
        {
            try
            {
                List<Model.RiskVehicle> risksVehicles = new List<Model.RiskVehicle>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
                filter.Like();
                filter.Constant(description + "%");

                PolicyVehicleRiskSummaryView policyVehicleSummaryView = new PolicyVehicleRiskSummaryView();
                ViewBuilder viewBuilder = new ViewBuilder("PolicyVehicleRiskSummaryView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, policyVehicleSummaryView);

                if (policyVehicleSummaryView.RiskVehicle.Count > 0)
                {
                    List<ISSEN.Risk> risks = policyVehicleSummaryView.Risks.Cast<ISSEN.Risk>().ToList();
                    List<ISSEN.Endorsement> endorsement = policyVehicleSummaryView.Endorsement.Cast<ISSEN.Endorsement>().ToList();
                    List<ISSEN.EndorsementRisk> endorsementRisk = policyVehicleSummaryView.EndorsementRisk.Cast<ISSEN.EndorsementRisk>().ToList();

                    risksVehicles = ModelAssembler.CreateRisksVehicles(policyVehicleSummaryView.RiskVehicle);

                    foreach (Model.RiskVehicle riskVehicle in risksVehicles)
                    {
                        riskVehicle.EndorsementId = endorsementRisk.FirstOrDefault(x => x.RiskId == riskVehicle.Risk.Id).EndorsementId;
                        riskVehicle.Risk.MainInsured.InsuredId = risks.FirstOrDefault(X => X.RiskId == riskVehicle.Risk.Id).InsuredId;
                        riskVehicle.Risk.CoveredRiskType = (CommonService.Enums.CoveredRiskType)risks.FirstOrDefault(x => x.RiskId == riskVehicle.Risk.Id).CoveredRiskTypeCode;
                    }
                }

                return risksVehicles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetRisks, ex);
            }
        }
    }
}
