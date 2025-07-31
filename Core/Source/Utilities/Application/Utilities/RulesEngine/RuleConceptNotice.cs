using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptNotice
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_CLAIM_NOTICE).ToString());


        #region Notice
        public static KeyValuePair<string, int> NoticeId => new KeyValuePair<string, int>("NoticeId", Id);
        public static KeyValuePair<string, int> ContactInformationName => new KeyValuePair<string, int>("ContactInformationName", Id);
        public static KeyValuePair<string, int> ContactInformationPhone => new KeyValuePair<string, int>("ContactInformationPhone", Id); 
        public static KeyValuePair<string, int> ContactInformationMail => new KeyValuePair<string, int>("ContactInformationMail", Id); 
        public static KeyValuePair<string, int> BranchCode => new KeyValuePair<string, int>("BranchCode", Id); 
        public static KeyValuePair<string, int> PrefixCode => new KeyValuePair<string, int>("PrefixCode", Id); 
        public static KeyValuePair<string, int> PolicyDocumentNumber => new KeyValuePair<string, int>("PolicyDocumentNumber", Id); 
        public static KeyValuePair<string, int> EndorsementNumber => new KeyValuePair<string, int>("EndorsementNumber", Id); 
        public static KeyValuePair<string, int> PolicyTypeId => new KeyValuePair<string, int>("PolicyTypeId", Id); 
        public static KeyValuePair<string, int> PolicyBusinessTypeId => new KeyValuePair<string, int>("PolicyBusinessTypeId", Id); 
        public static KeyValuePair<string, int> PolicyProductId => new KeyValuePair<string, int>("PolicyProductId", Id); 
        public static KeyValuePair<string, int> CurrentFrom => new KeyValuePair<string, int>("CurrentFrom", Id); 
        public static KeyValuePair<string, int> CurrentTo => new KeyValuePair<string, int>("CurrentTo", Id); 
        public static KeyValuePair<string, int> NoticeDate => new KeyValuePair<string, int>("NoticeDate", Id); 
        public static KeyValuePair<string, int> NoticeNumber => new KeyValuePair<string, int>("NoticeNumber", Id); 
        public static KeyValuePair<string, int> ObjetedDescription => new KeyValuePair<string, int>("ObjetedDescription", Id); 
        public static KeyValuePair<string, int> OccurrenceDate => new KeyValuePair<string, int>("OccurrenceDate", Id);
        public static KeyValuePair<string, int> DaysAfterNoticeAndOccurrence => new KeyValuePair<string, int>("DaysAfterNoticeAndOccurrence", Id);
        public static KeyValuePair<string, int> OccurrenceLocation => new KeyValuePair<string, int>("OccurrenceLocation", Id); 
        public static KeyValuePair<string, int> OccurrenceCountryId => new KeyValuePair<string, int>("OccurrenceCountryId", Id); 
        public static KeyValuePair<string, int> OccurrenceStateId => new KeyValuePair<string, int>("OccurrenceStateId", Id); 
        public static KeyValuePair<string, int> OccurrenceCityId => new KeyValuePair<string, int>("OccurrenceCityId", Id); 
        public static KeyValuePair<string, int> Description => new KeyValuePair<string, int>("Description", Id); 
        public static KeyValuePair<string, int> NoticeReasonId => new KeyValuePair<string, int>("NoticeReasonId", Id); 
        public static KeyValuePair<string, int> NoticeStateId => new KeyValuePair<string, int>("NoticeStateId", Id); 
        public static KeyValuePair<string, int> NoticeClaimedAmount => new KeyValuePair<string, int>("NoticeClaimedAmount", Id); 
        public static KeyValuePair<string, int> NoticeUserId => new KeyValuePair<string, int>("NoticeUserId", Id); 
        public static KeyValuePair<string, int> NoticeUserProfile => new KeyValuePair<string, int>("NoticeUserProfile", Id);         
        #endregion

        #region NoticeVehicle
        public static KeyValuePair<string, int> VehicleVersionId => new KeyValuePair<string, int>("VehicleVersionId", Id); 
        public static KeyValuePair<string, int> VehicleModelId => new KeyValuePair<string, int>("VehicleModelId", Id); 
        public static KeyValuePair<string, int> LicensePlateId => new KeyValuePair<string, int>("LicensePlateId", Id); 
        public static KeyValuePair<string, int> VehicleMakeId => new KeyValuePair<string, int>("VehicleMakeId", Id); 
        public static KeyValuePair<string, int> VehicleYear => new KeyValuePair<string, int>("VehicleYear", Id);
        public static KeyValuePair<string, int> ColorId => new KeyValuePair<string, int>("ColorId", Id);
        public static KeyValuePair<string, int> DamageTypeId => new KeyValuePair<string, int>("DamageTypeId", Id); 
        public static KeyValuePair<string, int> DamageResponsibilityId => new KeyValuePair<string, int>("DamageResponsibilityId", Id); 
        public static KeyValuePair<string, int> AditionalInformation => new KeyValuePair<string, int>("AditionalInformation", Id);         
        #endregion

        #region NoticeLocation
        public static KeyValuePair<string, int> RiskLocation => new KeyValuePair<string, int>("RiskLocation", Id); 
        public static KeyValuePair<string, int> CountryLocationId => new KeyValuePair<string, int>("CountryLocationId", Id); 
        public static KeyValuePair<string, int> StateLocationId => new KeyValuePair<string, int>("StateLocationId", Id); 
        public static KeyValuePair<string, int> CityLocationId => new KeyValuePair<string, int>("CityLocationId", Id);         
        #endregion

        #region NoticeSurety
        public static KeyValuePair<string, int> Surety => new KeyValuePair<string, int>("Surety", Id); 
        public static KeyValuePair<string, int> ContractNumberId => new KeyValuePair<string, int>("ContractNumberId", Id); 
        public static KeyValuePair<string, int> JudgmentNumberId => new KeyValuePair<string, int>("JudgmentNumberId", Id); 
        public static KeyValuePair<string, int> InsuredName => new KeyValuePair<string, int>("InsuredName", Id);         
        #endregion

        #region NoticeTransport
        public static KeyValuePair<string, int> MerchandiseTypeId => new KeyValuePair<string, int>("MerchandiseTypeId", Id); 
        public static KeyValuePair<string, int> PackagingTypeId => new KeyValuePair<string, int>("PackagingTypeId", Id); 
        public static KeyValuePair<string, int> OriginTransportId => new KeyValuePair<string, int>("OriginTransportId", Id); 
        public static KeyValuePair<string, int> DestinyTransportId => new KeyValuePair<string, int>("DestinyTransportId", Id); 
        public static KeyValuePair<string, int> TransportTypeId => new KeyValuePair<string, int>("TransportTypeId", Id);         
        #endregion

        #region NoticeAirCraft
        public static KeyValuePair<string, int> RegisterNumberId => new KeyValuePair<string, int>("RegisterNumberId", Id); 
        public static KeyValuePair<string, int> AirCraftMakeId => new KeyValuePair<string, int>("AirCraftMakeId", Id); 
        public static KeyValuePair<string, int> AirCraftModelId => new KeyValuePair<string, int>("AirCraftModelId", Id); 
        public static KeyValuePair<string, int> AirCraftUseId => new KeyValuePair<string, int>("AirCraftUseId", Id); 
        public static KeyValuePair<string, int> AirCraftRegisterId => new KeyValuePair<string, int>("AirCraftRegisterId", Id); 
        public static KeyValuePair<string, int> AirCraftOperatorId => new KeyValuePair<string, int>("AirCraftOperatorId", Id);         
        #endregion

        #region NoticeFidelity
        public static KeyValuePair<string, int> DiscoveryDate => new KeyValuePair<string, int>("DiscoveryDate", Id); 
        public static KeyValuePair<string, int> ActivityRiskId => new KeyValuePair<string, int>("ActivityRiskId", Id); 
        public static KeyValuePair<string, int> OccupationId => new KeyValuePair<string, int>("OccupationId", Id);         
        #endregion
        
        public static KeyValuePair<int, int> DynamicConcept(int id)
        {
            return new KeyValuePair<int, int>(id, Id);
        }
        public static KeyValuePair<int, int> DynamicConcept(int id, int entityId)
        {
            return new KeyValuePair<int, int>(id, entityId);
        }
    }
}
