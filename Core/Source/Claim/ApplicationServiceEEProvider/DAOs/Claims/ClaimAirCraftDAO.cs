using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimAirCraftDAO
    {
        public ClaimAirCraft CreateClaimAirCraft(ClaimAirCraft claimAirCraft)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimAirCraft.Claim = claimDAO.CreateClaim(claimAirCraft.Claim);
            
            return claimAirCraft;
        }

        public ClaimAirCraft UpdateClaimAirCraft(ClaimAirCraft claimAirCraft)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimAirCraft.Claim = claimDAO.UpdateClaim(claimAirCraft.Claim);

            return claimAirCraft;
        }
    }
}
