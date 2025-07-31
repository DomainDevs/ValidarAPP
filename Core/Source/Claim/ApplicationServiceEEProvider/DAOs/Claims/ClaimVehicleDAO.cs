using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimVehicleDAO
    {
        public ClaimVehicle CreateClaimVehicle(ClaimVehicle claimVehicle)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimVehicle.Claim = claimDAO.CreateClaim(claimVehicle.Claim);

            return claimVehicle;
        }

        public ClaimVehicle UpdateClaimVehicle(ClaimVehicle claimVehicle)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            claimVehicle.Claim = claimDAO.UpdateClaim(claimVehicle.Claim);

            return claimVehicle;
        }
    }
}
