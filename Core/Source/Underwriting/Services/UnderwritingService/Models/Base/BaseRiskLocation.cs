using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseRiskLocation
    {
 
        [DataMember]
        public decimal EmlPtc { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public int HouseNumber { get; set; }

        [DataMember]
        public string Floor { get; set; }

        [DataMember]
        public string Apartment { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Urbanization { get; set; }

        [DataMember]
        public bool IsMain { get; set; }

        [DataMember]
        public string AditionalStreet { get; set; }

        [DataMember]
        public string Block { get; set; }

        [DataMember]
        public int RiskAge { get; set; }

        [DataMember]
        public bool IsRetention { get; set; }

        [DataMember]
        public bool InspectionRecomendation { get; set; }

        [DataMember]
        public decimal LongitudeEarthquake { get; set; }

        [DataMember]
        public decimal LatitudEarthquake { get; set; }

        [DataMember]
        public decimal ConstructionYearEarthquake { get; set; }

        [DataMember]
        public decimal FloorNumberEarthquake { get; set; }

        [DataMember]
        public int HoustingTypeId { get; set; }

        [DataMember]
        public int CrestaZoneId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public int DeclarativePeriodId { get; set; }

        [DataMember]
        public int PremiumAdjustmentPeriodId { get; set; }

        [DataMember]
        public int InsuranceModeId { get; set; }

        [DataMember]
        public int RiskCommSubtypeId { get; set; }

        [DataMember]
        public int CommRiskClass { get; set; }

        [DataMember]
        public int RiskCommercialtype { get; set; }

        [DataMember]
        public int ConstructionCategoryId { get; set; }

        [DataMember]
        public int RiskDangerousId { get; set; }

        [DataMember]
        public int EconomicActivity { get; set; }

        [DataMember]
        public int RiskUse { get; set; }

        [DataMember]
        public int AddressType { get; set; }

        [DataMember]
        public int StreetType { get; set; }

        [DataMember]
        public int OccupationType { get; set; }
    }
}
