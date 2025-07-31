using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimLocationDAO
    {
        public ClaimLocation CreateClaimLocation(ClaimLocation claimLocation)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimLocation.Claim = claimDAO.CreateClaim(claimLocation.Claim);

            return claimLocation;
        }

        public ClaimLocation UpdateClaimLocation(ClaimLocation claimLocation)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimLocation.Claim = claimDAO.UpdateClaim(claimLocation.Claim);

            return claimLocation;
        }
    }
}
