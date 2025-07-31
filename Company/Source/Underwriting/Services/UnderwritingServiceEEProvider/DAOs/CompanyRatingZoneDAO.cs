using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CompanyRatingZoneDAO
    {
        public CompanyRatingZone GetCompanyRatingZoneByRatingZoneId(int ratingZoneId)
        {
            RatingZoneDAO ratingZoneDAO = new RatingZoneDAO();
            return ModelAssembler.CreateCompanyRatingZone(ratingZoneDAO.RatingZoneByRatingZoneCode(ratingZoneId));
        }
    }
}
