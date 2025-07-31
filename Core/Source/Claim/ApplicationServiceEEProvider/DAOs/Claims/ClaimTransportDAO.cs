using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimTransportDAO
    {
        public ClaimTransport CreateClaimTransport(ClaimTransport claimTransport)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimTransport.Claim = claimDAO.CreateClaim(claimTransport.Claim);
            
            return claimTransport;
        }

        public ClaimTransport UpdateClaimTransport(ClaimTransport claimTransport)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimTransport.Claim = claimDAO.UpdateClaim(claimTransport.Claim);

            return claimTransport;
        }

        public string GetDescriptionRiskTransportByRiskId(int riskId)
        {
            string Description = "";
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.RiskTransport).Name, riskId);

            RiskTransportView riskTransportView = new RiskTransportView();
            ViewBuilder viewBuilder = new ViewBuilder("RiskTransportView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskTransportView);

            if (riskTransportView.RiskTransports.Count > 0)
            {
                List<COMMEN.TransportMean> transportMeans = riskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().ToList();
                List<COMMEN.TransportCargoType> transportsCargoTypes = riskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().ToList();

                Description = transportsCargoTypes.FirstOrDefault().Description + " - " + transportMeans.FirstOrDefault().Description;
            }

            return Description;
        }
    }
}
