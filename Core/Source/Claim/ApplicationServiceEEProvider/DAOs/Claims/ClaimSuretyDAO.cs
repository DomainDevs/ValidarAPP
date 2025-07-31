using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimSuretyDAO
    {
        public ClaimSurety CreateClaimSurety(ClaimSurety claimSurety)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimSurety.Claim = claimDAO.CreateClaim(claimSurety.Claim);

            return claimSurety;
        }

        public ClaimSurety UpdateClaimSurety(ClaimSurety claimSurety)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimSurety.Claim = claimDAO.UpdateClaim(claimSurety.Claim);

            return claimSurety;
        }
    }
}
