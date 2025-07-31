using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptRisk
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_RISK).ToString());

        public static KeyValuePair<string, int> CoverageIdLast => new KeyValuePair<string, int>("CoverageIdLast", Id);

        public static KeyValuePair<string, int> EconomicActivityCode => new KeyValuePair<string, int>("EconomicActivityCode", Id);

        public static KeyValuePair<string, int> EmlPercentage => new KeyValuePair<string, int>("EmlPercentage", Id);

        public static KeyValuePair<string, int> CoveredRiskTypeCode => new KeyValuePair<string, int>("CoveredRiskTypeCode", Id);

        public static KeyValuePair<string, int> IsMain => new KeyValuePair<string, int>("IsMain", Id);

        public static KeyValuePair<string, int> CustomerTypeCode => new KeyValuePair<string, int>("CustomerTypeCode", Id);

        public static KeyValuePair<string, int> Street => new KeyValuePair<string, int>("Street", Id);

        public static KeyValuePair<string, int> ConstructionCategoryCode => new KeyValuePair<string, int>("ConstructionCategoryCode", Id);

        public static KeyValuePair<string, int> StateCode => new KeyValuePair<string, int>("StateCode", Id);

        public static KeyValuePair<string, int> AddressTypeCode => new KeyValuePair<string, int>("AddressTypeCode", Id);

        public static KeyValuePair<string, int> Urbanization => new KeyValuePair<string, int>("Urbanization", Id);

        public static KeyValuePair<string, int> CityCode => new KeyValuePair<string, int>("CityCode", Id);

        public static KeyValuePair<string, int> ZipCode => new KeyValuePair<string, int>("ZipCode", Id);

        public static KeyValuePair<string, int> Apartment => new KeyValuePair<string, int>("Apartment", Id);

        public static KeyValuePair<string, int> HouseNumber => new KeyValuePair<string, int>("HouseNumber", Id);

        public static KeyValuePair<string, int> RiskDangerousnessCode => new KeyValuePair<string, int>("RiskDangerousnessCode", Id);

        public static KeyValuePair<string, int> CrestaZoneCode => new KeyValuePair<string, int>("CrestaZoneCode", Id);

        public static KeyValuePair<string, int> StreetTypeCode => new KeyValuePair<string, int>("StreetTypeCode", Id);

        public static KeyValuePair<string, int> Floor => new KeyValuePair<string, int>("Floor", Id);

        public static KeyValuePair<string, int> EngineSerialNumber => new KeyValuePair<string, int>("EngineSerialNumber", Id);

        public static KeyValuePair<string, int> LoadTypeCode => new KeyValuePair<string, int>("LoadTypeCode", Id);

        public static KeyValuePair<string, int> TrailersQuantity => new KeyValuePair<string, int>("TrailersQuantity", Id);

        public static KeyValuePair<string, int> VehicleMakeCode => new KeyValuePair<string, int>("VehicleMakeCode", Id);

        public static KeyValuePair<string, int> LicensePlate => new KeyValuePair<string, int>("LicensePlate", Id);

        public static KeyValuePair<string, int> NewVehiclePrice => new KeyValuePair<string, int>("NewVehiclePrice", Id);

        public static KeyValuePair<string, int> IsNew => new KeyValuePair<string, int>("IsNew", Id);

        public static KeyValuePair<string, int> VehicleVersionCode => new KeyValuePair<string, int>("VehicleVersionCode", Id);

        public static KeyValuePair<string, int> ChassisSerialNumber => new KeyValuePair<string, int>("ChassisSerialNumber", Id);

        public static KeyValuePair<string, int> VehicleBodyCode => new KeyValuePair<string, int>("VehicleBodyCode", Id);

        public static KeyValuePair<string, int> VehicleTypeCode => new KeyValuePair<string, int>("VehicleTypeCode", Id);

        public static KeyValuePair<string, int> VehicleModelCode => new KeyValuePair<string, int>("VehicleModelCode", Id);

        public static KeyValuePair<string, int> VehiclePrice => new KeyValuePair<string, int>("VehiclePrice", Id);

        public static KeyValuePair<string, int> RatingZoneCode => new KeyValuePair<string, int>("RatingZoneCode", Id);

        public static KeyValuePair<string, int> PassengerQuantity => new KeyValuePair<string, int>("PassengerQuantity", Id);

        public static KeyValuePair<string, int> VehicleColorCode => new KeyValuePair<string, int>("VehicleColorCode", Id);

        public static KeyValuePair<string, int> VehicleUseCode => new KeyValuePair<string, int>("VehicleUseCode", Id);

        public static KeyValuePair<string, int> VehicleYear => new KeyValuePair<string, int>("VehicleYear", Id);

        public static KeyValuePair<string, int> IsTruck => new KeyValuePair<string, int>("IsTruck", Id);

        public static KeyValuePair<string, int> InsuredBirthDate => new KeyValuePair<string, int>("InsuredBirthDate", Id);

        public static KeyValuePair<string, int> InsuredAge => new KeyValuePair<string, int>("InsuredAge", Id);

        public static KeyValuePair<string, int> InsuredGender => new KeyValuePair<string, int>("InsuredGender", Id);

        public static KeyValuePair<string, int> IsInsuredPayer => new KeyValuePair<string, int>("IsInsuredPayer", Id);

        public static KeyValuePair<string, int> DrivingPercentage => new KeyValuePair<string, int>("DrivingPercentage", Id);

        public static KeyValuePair<string, int> YearsDriving => new KeyValuePair<string, int>("YearsDriving", Id);

        public static KeyValuePair<string, int> DriverId => new KeyValuePair<string, int>("DriverId", Id);

        public static KeyValuePair<string, int> DriverNumber => new KeyValuePair<string, int>("DriverNumber", Id);

        public static KeyValuePair<string, int> LicenseExpirationDate => new KeyValuePair<string, int>("LicenseExpirationDate", Id);

        public static KeyValuePair<string, int> LicenseCategory => new KeyValuePair<string, int>("LicenseCategory", Id);

        public static KeyValuePair<string, int> VehicleDriverBirthDate => new KeyValuePair<string, int>("VehicleDriverBirthDate", Id);

        public static KeyValuePair<string, int> VehicleDriverAge => new KeyValuePair<string, int>("VehicleDriverAge", Id);

        public static KeyValuePair<string, int> VehicleDriverMaritalStatusCode => new KeyValuePair<string, int>("VehicleDriverMaritalStatusCode", Id);

        public static KeyValuePair<string, int> VehicleDriverGender => new KeyValuePair<string, int>("VehicleDriverGender", Id);

        public static KeyValuePair<string, int> BeneficiaryId => new KeyValuePair<string, int>("BeneficiaryId", Id);

        public static KeyValuePair<string, int> BeneficiaryTypeCode => new KeyValuePair<string, int>("BeneficiaryTypeCode", Id);

        public static KeyValuePair<string, int> BeneficiaryPercentage => new KeyValuePair<string, int>("BeneficiaryPercentage", Id);

        public static KeyValuePair<string, int> ConditionText => new KeyValuePair<string, int>("ConditionText", Id);

        public static KeyValuePair<string, int> EndorsementId => new KeyValuePair<string, int>("EndorsementId", Id);

        public static KeyValuePair<string, int> InsuredId => new KeyValuePair<string, int>("InsuredId", Id);

        public static KeyValuePair<string, int> RiskNumber => new KeyValuePair<string, int>("RiskNumber", Id);

        public static KeyValuePair<string, int> RiskStatusCode => new KeyValuePair<string, int>("RiskStatusCode", Id);

        public static KeyValuePair<string, int> RiskOriginalStatusCode => new KeyValuePair<string, int>("RiskOriginalStatusCode", Id);

        public static KeyValuePair<string, int> IsDefault => new KeyValuePair<string, int>("IsDefault", Id);

        public static KeyValuePair<string, int> AverageValue => new KeyValuePair<string, int>("AverageValue", Id);

        public static KeyValuePair<string, int> HasNew => new KeyValuePair<string, int>("HasNew", Id);

        public static KeyValuePair<string, int> HasUsed => new KeyValuePair<string, int>("HasUsed", Id);

        public static KeyValuePair<string, int> CoverageGroupId => new KeyValuePair<string, int>("CoverageGroupId", Id);

        public static KeyValuePair<string, int> HousingTypeId => new KeyValuePair<string, int>("HousingTypeId", Id);

        public static KeyValuePair<string, int> CommRiskClassId => new KeyValuePair<string, int>("CommRiskClassId", Id);

        public static KeyValuePair<string, int> RiskCommercialTypeId => new KeyValuePair<string, int>("RiskCommercialTypeId", Id);

        public static KeyValuePair<string, int> RiskCommSubtypeId => new KeyValuePair<string, int>("RiskCommSubtypeId", Id);

        public static KeyValuePair<string, int> VehicleFuelCode => new KeyValuePair<string, int>("VehicleFuelCode", Id);

        public static KeyValuePair<string, int> OcupationTypeId => new KeyValuePair<string, int>("OcupationTypeId", Id);

        public static KeyValuePair<string, int> RiskId => new KeyValuePair<string, int>("RiskId", Id);

        public static KeyValuePair<string, int> TempId => new KeyValuePair<string, int>("TempId", Id);

        public static KeyValuePair<string, int> HasWarrantCreditor => new KeyValuePair<string, int>("HasWarrantCreditor", Id);

        public static KeyValuePair<string, int> DriversQuantity => new KeyValuePair<string, int>("DriversQuantity", Id);

        public static KeyValuePair<string, int> StandardVehiclePrice => new KeyValuePair<string, int>("StandardVehiclePrice", Id);

        public static KeyValuePair<string, int> InsuredZipCode => new KeyValuePair<string, int>("InsuredZipCode", Id);

        public static KeyValuePair<string, int> InsuredStateCode => new KeyValuePair<string, int>("InsuredStateCode", Id);

        public static KeyValuePair<string, int> IAPassengerQuantity => new KeyValuePair<string, int>("IAPassengerQuantity", Id);

        public static KeyValuePair<string, int> CountryCode => new KeyValuePair<string, int>("CountryCode", Id);

        public static KeyValuePair<string, int> DriverLicenseVehicleTypeCode => new KeyValuePair<string, int>("DriverLicenseVehicleTypeCode", Id);

        public static KeyValuePair<string, int> RiskInspectionTypeCode => new KeyValuePair<string, int>("RiskInspectionTypeCode", Id);

        public static KeyValuePair<string, int> PaymentMethodCode => new KeyValuePair<string, int>("PaymentMethodCode", Id);

        public static KeyValuePair<string, int> PayerId => new KeyValuePair<string, int>("PayerId", Id);

        public static KeyValuePair<string, int> FlatRatePercentage => new KeyValuePair<string, int>("FlatRatePercentage", Id);

        public static KeyValuePair<string, int> LimitsRcCode => new KeyValuePair<string, int>("LimitsRcCode", Id);

        public static KeyValuePair<string, int> SuretyContractType => new KeyValuePair<string, int>("SuretyContractType", Id);

        public static KeyValuePair<string, int> IndividualId => new KeyValuePair<string, int>("IndividualId", Id);

        public static KeyValuePair<string, int> SuretyContractCategoriesCode => new KeyValuePair<string, int>("SuretyContractCategoriesCode", Id);

        public static KeyValuePair<string, int> ContractAmount => new KeyValuePair<string, int>("ContractAmount", Id);

        public static KeyValuePair<string, int> OperatingQuotaAmount => new KeyValuePair<string, int>("OperatingQuotaAmount", Id);

        public static KeyValuePair<string, int> OperatingPile => new KeyValuePair<string, int>("OperatingPile", Id);

        public static KeyValuePair<string, int> ProspectAge => new KeyValuePair<string, int>("ProspectAge", Id);

        public static KeyValuePair<string, int> ProspectGender => new KeyValuePair<string, int>("ProspectGender", Id);

        public static KeyValuePair<string, int> PolicyTypeViewCode => new KeyValuePair<string, int>("PolicyTypeViewCode", Id);

        public static KeyValuePair<string, int> LimitsRcSum => new KeyValuePair<string, int>("LimitsRcSum", Id);

        public static KeyValuePair<string, int> Excess => new KeyValuePair<string, int>("Excess", Id);

        public static KeyValuePair<string, int> ServiceTypeCode => new KeyValuePair<string, int>("ServiceTypeCode", Id);

        public static KeyValuePair<string, int> ShuttleCode => new KeyValuePair<string, int>("ShuttleCode", Id);

        public static KeyValuePair<string, int> DeductId => new KeyValuePair<string, int>("DeductId", Id);

        public static KeyValuePair<string, int> PassengerQuantityAircraft => new KeyValuePair<string, int>("PassengerQuantityAircraft", Id);

        public static KeyValuePair<string, int> CrewQuantity => new KeyValuePair<string, int>("CrewQuantity", Id);

        public static KeyValuePair<string, int> LoadCapacity => new KeyValuePair<string, int>("LoadCapacity", Id);

        public static KeyValuePair<string, int> AircraftYear => new KeyValuePair<string, int>("AircraftYear", Id);

        public static KeyValuePair<string, int> AircraftMakeCode => new KeyValuePair<string, int>("AircraftMakeCode", Id);

        public static KeyValuePair<string, int> AircraftModelCode => new KeyValuePair<string, int>("AircraftModelCode", Id);

        public static KeyValuePair<string, int> AircraftTypeCode => new KeyValuePair<string, int>("AircraftTypeCode", Id);

        public static KeyValuePair<string, int> AircraftUseCode => new KeyValuePair<string, int>("AircraftUseCode", Id);

        public static KeyValuePair<string, int> AircraftTerritoryCode => new KeyValuePair<string, int>("AircraftTerritoryCode", Id);

        public static KeyValuePair<string, int> MaterialHullCode => new KeyValuePair<string, int>("MaterialHullCode", Id);

        public static KeyValuePair<string, int> MotorTypeCode => new KeyValuePair<string, int>("MotorTypeCode", Id);

        public static KeyValuePair<string, int> ContractAmountPercentage => new KeyValuePair<string, int>("ContractAmountPercentage", Id);

        public static KeyValuePair<string, int> IsRetention => new KeyValuePair<string, int>("IsRetention", Id);

        public static KeyValuePair<string, int> RiskAge => new KeyValuePair<string, int>("RiskAge", Id);

        public static KeyValuePair<string, int> RiskTypeCode => new KeyValuePair<string, int>("RiskTypeCode", Id);

        public static KeyValuePair<string, int> InspectionRecomendation => new KeyValuePair<string, int>("InspectionRecomendation", Id);

        public static KeyValuePair<string, int> AnnouncementDate => new KeyValuePair<string, int>("AnnouncementDate", Id);

        public static KeyValuePair<string, int> ReleaseDate => new KeyValuePair<string, int>("ReleaseDate", Id);

        public static KeyValuePair<string, int> Source => new KeyValuePair<string, int>("Source", Id);

        public static KeyValuePair<string, int> Destiny => new KeyValuePair<string, int>("Destiny", Id);

        public static KeyValuePair<string, int> CountrySourceId => new KeyValuePair<string, int>("CountrySourceId", Id);

        public static KeyValuePair<string, int> CitySourceId => new KeyValuePair<string, int>("CitySourceId", Id);
        public static KeyValuePair<string, int> StateSourceId => new KeyValuePair<string, int>("StateSourceId", Id);
        public static KeyValuePair<string, int> CountryDestinyId => new KeyValuePair<string, int>("CountryDestinyId", Id);
        public static KeyValuePair<string, int> CityDestinyId => new KeyValuePair<string, int>("CityDestinyId", Id);
        public static KeyValuePair<string, int> StateDestinyId => new KeyValuePair<string, int>("StateDestinyId", Id);

        public static KeyValuePair<string, int> AdjustPeriodId => new KeyValuePair<string, int>("AdjustPeriodId", Id);

        public static KeyValuePair<string, int> CustomsAgentId => new KeyValuePair<string, int>("CustomsAgentId", Id);

        public static KeyValuePair<string, int> DeclarationPeriodId => new KeyValuePair<string, int>("DeclarationPeriodId", Id);

        public static KeyValuePair<string, int> TransportCargoTypeId => new KeyValuePair<string, int>("TransportCargoTypeId", Id);

        public static KeyValuePair<string, int> TransportPackagingTypeId => new KeyValuePair<string, int>("TransportPackagingTypeId", Id);

        public static KeyValuePair<string, int> TransportViaTypeId => new KeyValuePair<string, int>("TransportViaTypeId", Id);
        public static KeyValuePair<string, int> FreightAmount => new KeyValuePair<string, int>("FreightAmount", Id);

        public static KeyValuePair<string, int> ReleaseAmount => new KeyValuePair<string, int>("ReleaseAmount", Id);

        public static KeyValuePair<string, int> HolderTypeCode => new KeyValuePair<string, int>("HolderTypeCode", Id);

        public static KeyValuePair<string, int> LimitMaxReleaseAmount => new KeyValuePair<string, int>("LimitMaxReleaseAmount", Id);

        public static KeyValuePair<string, int> RateTypeCode => new KeyValuePair<string, int>("RateTypeCode", Id);

        public static KeyValuePair<string, int> BeneficiaryIdentificationDocument => new KeyValuePair<string, int>("BeneficiaryIdentificationDocument", Id);

        public static KeyValuePair<string, int> InsuredIdentificationDocument => new KeyValuePair<string, int>("InsuredIdentificationDocument", Id);

        public static KeyValuePair<string, int> PremiunRisk => new KeyValuePair<string, int>("Premium", Id);

        public static KeyValuePair<string, int> AmountInsured => new KeyValuePair<string, int>("AmountInsured", Id);

        public static KeyValuePair<string, int> CaucionInsuredActAs => new KeyValuePair<string, int>("CaucionInsuredActAs", Id);

        public static KeyValuePair<string, int> CaucionHolderActAs => new KeyValuePair<string, int>("CaucionHolderActAs", Id);

        public static KeyValuePair<string, int> CaucionSettledNumber => new KeyValuePair<string, int>("CaucionSettledNumber", Id);

        public static KeyValuePair<string, int> CaucionInsuredValue => new KeyValuePair<string, int>("CaucionInsuredValue", Id);

        public static KeyValuePair<string, int> CaucionDepartment => new KeyValuePair<string, int>("CaucionDepartment", Id);

        public static KeyValuePair<string, int> CaucionCity => new KeyValuePair<string, int>("CaucionCity", Id);

        public static KeyValuePair<string, int> CaucionCourtType => new KeyValuePair<string, int>("CaucionCourtType", Id);

        public static KeyValuePair<string, int> OperationId => new KeyValuePair<string, int>("OperationId", Id);

        public static KeyValuePair<string, int> ConditionTextId => new KeyValuePair<string, int>("ConditionTextId", Id);

        public static KeyValuePair<string, int> ClausesAdd => new KeyValuePair<string, int>("ClausesAdd", Id);

        public static KeyValuePair<string, int> ClausesRemove => new KeyValuePair<string, int>("ClausesRemove", Id);

        public static KeyValuePair<string, int> CoveragesAdd => new KeyValuePair<string, int>("CoveragesAdd", Id);

        public static KeyValuePair<string, int> CoveragesRemove => new KeyValuePair<string, int>("CoveragesRemove", Id);

        public static KeyValuePair<string, int> CoveragesCount => new KeyValuePair<string, int>("CoveragesCount", Id);

        public static KeyValuePair<string, int> Coverages => new KeyValuePair<string, int>("Coverages", Id);

        public static KeyValuePair<string, int> BeneficiariesCountNoOneroso => new KeyValuePair<string, int>("BeneficiariesCountNoOneroso", Id);

        public static KeyValuePair<string, int> BeneficiariesCountOneroso => new KeyValuePair<string, int>("BeneficiariesCountOneroso", Id);

        public static KeyValuePair<string, int> BeneficiariesCount => new KeyValuePair<string, int>("BeneficiariesCount", Id);

        public static KeyValuePair<string, int> Beneficiaries => new KeyValuePair<string, int>("Beneficiaries", Id);

        public static KeyValuePair<string, int> DeductCoverages => new KeyValuePair<string, int>("DeductCoverages", Id);

        public static KeyValuePair<string, int> IsConsortium => new KeyValuePair<string, int>("IsConsortium", Id);

        public static KeyValuePair<string, int> ContractorInsuredCode => new KeyValuePair<string, int>("ContractorInsuredCode", Id);
        public static KeyValuePair<string, int> LimitAmount => new KeyValuePair<string, int>("LimitAmount", Id);
        public static KeyValuePair<string, int> InsuredDocumentNumber => new KeyValuePair<string, int>("InsuredDocumentNumber", Id);
        public static KeyValuePair<string, int> TypeOfInsuredDocument => new KeyValuePair<string, int>("TypeOfInsuredDocument", Id);
        public static KeyValuePair<string, int> TypeOfInsuredPerson => new KeyValuePair<string, int>("TypeOfInsuredPerson", Id);

        public static KeyValuePair<string, int> NameInsured => new KeyValuePair<string, int>("NameInsured", Id);
        public static KeyValuePair<string, int> NumberDocumentBeneficiary => new KeyValuePair<string, int>("NumberDocumentBeneficiary", Id);
        public static KeyValuePair<string, int> NameBeneficiary => new KeyValuePair<string, int>("NameBeneficiary", Id);

        public static KeyValuePair<string, int> InsuredValueGuarantee => new KeyValuePair<string, int>("InsuredValueGuarantee", Id);

        public static KeyValuePair<string, int> OpenGuarantee => new KeyValuePair<string, int>("OpenGuarantee", Id);

        public static KeyValuePair<string, int> GuaranteeStatus => new KeyValuePair<string, int>("GuaranteeStatus", Id);

        public static KeyValuePair<string, int> GuaranteeId => new KeyValuePair<string, int>("GuaranteeId", Id);

        public static KeyValuePair<string, int> IndividualBeneficiary => new KeyValuePair<string, int>("IndividualBeneficiary", Id);

        public static KeyValuePair<string, int> IndividualInsured => new KeyValuePair<string, int>("IndividualInsured", Id);

        public static KeyValuePair<string, int> IndividualOfTheBond => new KeyValuePair<string, int>("IndividualOfTheBond", Id);

        public static KeyValuePair<string, int> InsuredDocumentNumberOfTheBond => new KeyValuePair<string, int>("InsuredDocumentNumberOfTheBond", Id);

        public static KeyValuePair<string, int> InsuredNameOfTheBond => new KeyValuePair<string, int>("InsuredNameOfTheBond", Id);

        public static KeyValuePair<string, int> TypeOfBeneficiaryDocument => new KeyValuePair<string, int>("TypeOfBeneficiaryDocument", Id);

        public static KeyValuePair<string, int> TypeOfTheBondDocument => new KeyValuePair<string, int>("TypeOfTheBondDocument", Id);

        public static KeyValuePair<string, int> RcRiskActivity => new KeyValuePair<string, int>("RcRiskActivity", Id);

        public static KeyValuePair<string, int> InsuranceMode => new KeyValuePair<string, int>("InsuranceMode", Id);

        public static KeyValuePair<int, int> DynamicConcept(int id)
        {
            return new KeyValuePair<int, int>(id, Id);
        }
        public static KeyValuePair<int, int> DynamicConcept(int id, int entityId)
        {
            return new KeyValuePair<int, int>(id, entityId);
        }

        public static KeyValuePair<string, int> ContractorId => new KeyValuePair<string, int>("ContractorId", Id);
        public static KeyValuePair<string, int> VehicleGallonTankCapacity => new KeyValuePair<string, int>("VehicleGallonTankCapacity", Id);

        public static KeyValuePair<string, int> Guarantees => new KeyValuePair<string, int>("Guarantees", Id);

        public static KeyValuePair<string, int> JudRiskActivity => new KeyValuePair<string, int>("JudRiskActivity", Id);


        public static KeyValuePair<string, int> InsuredAssociationType => new KeyValuePair<string, int>("InsuredAssociationType", Id);

        public static KeyValuePair<string, int> ContractorAssociationType => new KeyValuePair<string, int>("ContractorAssociationType", Id);
    }
}