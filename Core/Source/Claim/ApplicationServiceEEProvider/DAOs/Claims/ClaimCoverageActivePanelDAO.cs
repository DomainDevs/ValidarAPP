using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimCoverageActivePanelDAO
    {
        public List<ClaimCoverageActivePanel> GetClaimCoverageActivePanelsByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(QUOEN.Coverage).Name);
            filter.Equal();
            filter.Constant(subLineBusinessId);

            ClaimCoverageActivePanelView claimCoverageActivePanelView = new ClaimCoverageActivePanelView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimCoverageActivePanelView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimCoverageActivePanelView);

            if (claimCoverageActivePanelView.ClaimCoverageActivePanels.Count > 0)
            {
                List<ClaimCoverageActivePanel> claimCoverageActivePanels = new List<ClaimCoverageActivePanel>();
                foreach (PARAMEN.ClaimCoverageActivePanel entityClaimCoverageActivePanel in claimCoverageActivePanelView.ClaimCoverageActivePanels)
                {
                    ClaimCoverageActivePanel claimCoverageActivePanel = ModelAssembler.CreateClaimCoverageActivePanel(entityClaimCoverageActivePanel);
                    claimCoverageActivePanel.CoverageDescription = claimCoverageActivePanelView.Coverages.Cast<QUOEN.Coverage>().First(x => x.CoverageId == entityClaimCoverageActivePanel.CoverageId).PrintDescription;
                    claimCoverageActivePanels.Add(claimCoverageActivePanel);
                }

                return claimCoverageActivePanels;
            }
            else
            {
                return new List<ClaimCoverageActivePanel>();
            }
        }

        public ClaimCoverageActivePanel CreateClaimCoverageActivePanel(ClaimCoverageActivePanel claimCoverageActivePanel)
        {
            PARAMEN.ClaimCoverageActivePanel entityClaimCoverageActivePanel = EntityAssembler.CreateClaimCoverageActivePanel(claimCoverageActivePanel);

            DataFacadeManager.Insert(entityClaimCoverageActivePanel);
            return ModelAssembler.CreateClaimCoverageActivePanel(entityClaimCoverageActivePanel);
        }

        public ClaimCoverageActivePanel UpdateClaimCoverageActivePanel(ClaimCoverageActivePanel ClaimCoverageActivePanel)
        {
            PrimaryKey primaryKey = PARAMEN.ClaimCoverageActivePanel.CreatePrimaryKey(ClaimCoverageActivePanel.CoverageId);

            PARAMEN.ClaimCoverageActivePanel entityClaimCoverageActivePanel = (PARAMEN.ClaimCoverageActivePanel)DataFacadeManager.GetObject(primaryKey);

            entityClaimCoverageActivePanel.EnabledDriver = ClaimCoverageActivePanel.EnabledDriver;
            entityClaimCoverageActivePanel.EnabledThirdPartyVehicle = ClaimCoverageActivePanel.EnabledThirdPartyVehicle;
            entityClaimCoverageActivePanel.EnabledThird = ClaimCoverageActivePanel.EnabledThird;
            entityClaimCoverageActivePanel.EnabledAffectedProperty = ClaimCoverageActivePanel.EnabledAffectedProperty;

            DataFacadeManager.Update(entityClaimCoverageActivePanel);

            return ModelAssembler.CreateClaimCoverageActivePanel(entityClaimCoverageActivePanel);
        }

        public ClaimCoverageActivePanel GetActivePanelsByCoverageId(int coverageId)
        {
            PrimaryKey primaryKey = PARAMEN.ClaimCoverageActivePanel.CreatePrimaryKey(coverageId);
            PARAMEN.ClaimCoverageActivePanel entityClaimCoverageActivePanel = (PARAMEN.ClaimCoverageActivePanel)DataFacadeManager.GetObject(primaryKey);

            ClaimCoverageActivePanel claimCoverageActivePanels = ModelAssembler.CreateClaimCoverageActivePanel(entityClaimCoverageActivePanel);

            return claimCoverageActivePanels;
        }
    }
}
