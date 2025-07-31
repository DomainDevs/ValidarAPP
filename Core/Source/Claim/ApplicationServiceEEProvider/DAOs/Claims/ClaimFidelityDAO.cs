using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimFidelityDAO
    {
        public ClaimFidelity CreateClaimFidelity(ClaimFidelity claimFidelity)
        {
            ClaimDAO claimDAO = new ClaimDAO();

            claimFidelity.Claim = claimDAO.CreateClaim(claimFidelity.Claim);

            return claimFidelity;
        }

        public ClaimFidelity UpdateClaimFidelity(ClaimFidelity claimFidelity)
        {
            ClaimDAO claimDAO = new ClaimDAO();

            claimFidelity.Claim = claimDAO.UpdateClaim(claimFidelity.Claim);

            return claimFidelity;
        }
    }
}
