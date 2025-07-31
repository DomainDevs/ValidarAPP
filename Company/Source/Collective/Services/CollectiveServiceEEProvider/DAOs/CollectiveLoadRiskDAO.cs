using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.CollectiveServices.EEProvider.DAOs
{
    public class CollectiveLoadRiskDAO
    {
        public GroupCoverage CreateGroupCoverage(Row row, int productId)
        {
            int riskGroupCoverageId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskGroupCoverage));
            List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetGroupCoverages(productId);
            if (riskGroupCoverageId > 0)
            {
                return groupCoverages.Find(x => x.Id == riskGroupCoverageId);
            }
            return groupCoverages.FirstOrDefault();            
        }

        public RatingZone CreateRatingZone(Row row, int prefixId)
        {
            int ratingZoneId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.First(y => y.PropertyName == FieldPropertyName.RiskRatingZone));
            List<RatingZone> ratingZones = DelegateService.underwritingService.GetRatingZonesByPrefixId(prefixId);
            if (ratingZoneId > 0)
            {
                return ratingZones.Find(x => x.Id == ratingZoneId);
            }
            return ratingZones.FirstOrDefault();            
        }

        public List<CompanyCoverage> CreateCoverages(int productId, int groupCoverageId, int prefixId)
        {
            return DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
        }
        
        public LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId)
        {
            int limitRc = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc));
            return DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId).First(x => x.Id == limitRc);
        }
    }
}