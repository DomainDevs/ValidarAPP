using Sistran.Company.Application.ModelServices.Models;
using Sistran.Company.Application.ModelServices.Models.Reports;
using Sistran.Company.ExternalParamService.Models;
using Sistran.Company.ExternalParamServicesEEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.DAF;
using SIstran.Company.ExternalParamService.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Company.ExternalParamServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static List<QuoteParamIdentityCardClass> GenerateIdentityCardTypeList(DocumentTypesServiceModel documentTypes)
        {
            List<QuoteParamIdentityCardClass> quoteParamIdentityCardClass = new List<QuoteParamIdentityCardClass>();
            foreach (var item in documentTypes.DocumentTypeServiceModel)
            {
                quoteParamIdentityCardClass.Add(GenerateIdentityCardType(item));
            }
            quoteParamIdentityCardClass.Add(new QuoteParamIdentityCardClass { ProcessMessage = Errors.ProcessSuccess });

            return quoteParamIdentityCardClass;
        }

        public static List<ParamAddressType> GetAddressesTypes(List<AddressType> addressType)
        {

            List<ParamAddressType> paramAddressType = new List<ParamAddressType>();
            foreach (var item in addressType)
            {
                paramAddressType.Add(GetAddressesType(item));
            }
            paramAddressType.Add(new ParamAddressType { ProcessMessage = Errors.ProcessSuccess });

            return paramAddressType;
        }

        public static List<ParamCompanyType> GetCompanyTypes(List<CompanyType> companyTypes)
        {

            List<ParamCompanyType> listCompanyTypes = new List<ParamCompanyType>();
            foreach (var item in companyTypes)
            {
                listCompanyTypes.Add(GetCompanyType(item));
            }
            listCompanyTypes.Add(new ParamCompanyType { ProcessMessage = Errors.ProcessSuccess });

            return listCompanyTypes;
        }

        public static List<ParamEconomicActivity> GetEconomicActivities(List<EconomicActivity> economicActivity)
        {
            List<ParamEconomicActivity> getEconomicActivities = new List<ParamEconomicActivity>();
            foreach (var item in economicActivity)
            {
                getEconomicActivities.Add(GetEconomicActivitie(item));
            }
            getEconomicActivities.Add(new ParamEconomicActivity { ProcessMessage = Errors.ProcessSuccess });

            return getEconomicActivities;
        }

        public static List<ParamListCountryStateCity> GenerateCountryStateCity(List<CountryStateCityServiceModel> countryStateCityServiceModel)
        {
            List<ParamListCountryStateCity> paramListCountryStateCity = new List<ParamListCountryStateCity>();
            foreach (var item in countryStateCityServiceModel)
            {
                paramListCountryStateCity.Add(GetCountryStateCity(item));
            }
            paramListCountryStateCity.Add(new ParamListCountryStateCity { ProcessMessage = Errors.ProcessSuccess });

            return paramListCountryStateCity;
        }

        public static List<QuoteParamRatingZoneClass> QuoteParamRatingZone(List<RatingZone> listRatingZone)
        {
            List<QuoteParamRatingZoneClass> quoteParamRatingZoneClass = new List<QuoteParamRatingZoneClass>();
            foreach (var ratingZone in listRatingZone)
            {
                quoteParamRatingZoneClass.Add(QuoteParamRatingZoneClass(ratingZone));
            }
            quoteParamRatingZoneClass.Add(new QuoteParamRatingZoneClass { ProcessMessage = Errors.ProcessSuccess });

            return quoteParamRatingZoneClass;
        }

        public static List<PhoneTypeClass> ListPhoneTypeClass(List<PhoneType> phoneTypes)
        {
            List<PhoneTypeClass> phoneTypeClass = new List<PhoneTypeClass>();
            foreach (var item in phoneTypes)
            {
                phoneTypeClass.Add(PhoneType(item));
            }

            return phoneTypeClass;
        }

        public static List<ParamListProduct> GenerateListProduct(List<GroupCoverage> getGroupCoveragesByPrefixCd)
        {
            List<ParamListProduct> paramListProducts = new List<ParamListProduct>();

            foreach (var item in getGroupCoveragesByPrefixCd)
            {
                paramListProducts.Add(getGroupCoverageByPrefixCd(item));
            }

            return paramListProducts;
        }

        private static ParamListProduct getGroupCoverageByPrefixCd(GroupCoverage item) => new ParamListProduct
        {
            ListProductClass = new List<ListProductClass>
            {
                new ListProductClass
                {
                    PrefixCd = item.Product.Prefix.Id,
                    DescriptionPrefix = "",
                    ProductCd = item.Product.Id,
                    DescriptionProduct = "",
                    GroupCoverageCd = item.Id,
                    DescriptionGroup = item.Description
                }
            }
        };

        public static List<ParamListDetail> GenerateListDetail(List<Accessory> accessoryDescriptions)
        {
            List<ParamListDetail> paramListDetail = new List<ParamListDetail>();
            foreach (var item in accessoryDescriptions)
            {
                paramListDetail.Add(GenerateDetail(item));
            }
            paramListDetail.Add(new ParamListDetail { ProcessMessage = Errors.ProcessSuccess });

            return paramListDetail;
        }

        public static QuoteParamVehicleFasecoldaClass GenerateVehicleFasecolda(List<VehicleParameterServiceModel> vehiclesParameters)
        {
            QuoteParamVehicleFasecoldaClass quoteParamVehicleFasecoldaClass = new QuoteParamVehicleFasecoldaClass();

            foreach (var item in vehiclesParameters)
            {
                quoteParamVehicleFasecoldaClass.VehicleFasecolda = new ExternalParamService.DTO.VehicleParameterDTO
                {
                    FasecoldaMakeId = item.FasecoldaMakeId,
                    FasecoldaModelId = item.FasecoldaModelId,
                    MakeDescription = item.MakeDescription,
                    ModelDescription = item.ModelDescription,
                    VehicleMakeCode = item.VehicleMakeCode,
                    VehicleModelCode = item.VehicleModelCode,
                    VehiclePrice = item.VehiclePrice,
                    VehicleTypeCode = item.VehicleTypeCode,
                    VehicleTypeDescription = item.VehicleTypeDescription,
                    VehicleVersionCode = item.VehicleVersionCode,
                    VehicleYear = item.VehicleYear,
                    VersionDescription = item.VersionDescription
                };
            }

            return quoteParamVehicleFasecoldaClass;
        }

        private static ParamListDetail GenerateDetail(Accessory item) => new ParamListDetail
        {
            ListDetailList = new List<ListDetailClass>
            {
                new ListDetailClass
                {
                    AccesoryCode = item.Id,
                    Description = item.Description
                }
            }
        };

        private static QuoteParamRatingZoneClass QuoteParamRatingZoneClass(RatingZone ratingZone) => new QuoteParamRatingZoneClass
        {
            RatingZoneClass = new List<RatingZoneClass>
            {
                new RatingZoneClass
                {
                    RatingZoneCode = ratingZone.Id,
                    Description = ratingZone.Description,
                    SmallDescription = ratingZone.SmallDescription,
                    PrefixCode = ratingZone.Prefix.Id,
                    IsDefault = false
                }
            }
        };

        public static AgentModel GetAgents(List<Agency> agencies)
        {
            AgentModel agentModel = new AgentModel();
            foreach (var item in agencies)
            {
                agentModel.IndividualTypeCd = item.Code;
                agentModel.Message = Errors.ProcessSuccess;
                agentModel.MotherLastName = item.Agent.EmployeePerson?.MotherLastName;
                agentModel.Name = item.Agent.EmployeePerson?.Name;
                agentModel.TradeName = item.Agent.FullName;                
            }

            return agentModel;
        }

        private static PhoneTypeClass PhoneType(PhoneType item) => new PhoneTypeClass
        {
            Description = item.Description,
            PhoneTypeCd = item.Id
        };

        public static List<QuoteParamVehicleClass> VehiclesParams(VehiclesParametersServiceModel vehiclesParameters)
        {
            List<QuoteParamVehicleClass> quoteParamVehicleClasses = new List<QuoteParamVehicleClass>();
            foreach (var item in vehiclesParameters.vehicleParameter)
            {
                quoteParamVehicleClasses.Add(VehicleParam(item));
            }

            return quoteParamVehicleClasses;
        }

        private static QuoteParamVehicleClass VehicleParam(VehicleParameterServiceModel item) => new QuoteParamVehicleClass
        {
            VehicleParameterDTOCo = new List<ExternalParamService.DTO.VehicleParameterDTO>
            {
                new ExternalParamService.DTO.VehicleParameterDTO
                {
                    FasecoldaMakeId = item.FasecoldaMakeId,
                    FasecoldaModelId = item.FasecoldaModelId,
                    MakeDescription = item.MakeDescription,
                    ModelDescription = item.ModelDescription,
                    VehicleMakeCode = item.VehicleMakeCode,
                    VehicleModelCode = item.VehicleModelCode,
                    VehiclePrice = item.VehiclePrice,
                    VehicleTypeCode = item.VehicleTypeCode,
                    VehicleTypeDescription = item.VehicleTypeDescription,
                    VehicleVersionCode = item.VehicleVersionCode,
                    VehicleYear = item.VehicleYear,
                    VersionDescription = item.VersionDescription
                }
            }
        };

        private static ParamEconomicActivity GetEconomicActivitie(EconomicActivity item) => new ParamEconomicActivity
        {
            EconomicActivityClass = new List<EconomicActivityClass>
            {
                new EconomicActivityClass
                {
                    Description = item.Description,
                    EconomicActivityCd = item.Id
                }
            }
        };

        private static ParamCompanyType GetCompanyType(CompanyType item) => new ParamCompanyType
        {
            CompanyTypeClass = new List<CompanyTypeClass>
           {
               new CompanyTypeClass
               {
                   CompanyTypeCd = item.Id,
                   Description = item.Description
               }
           }
        };

        private static ParamListCountryStateCity GetCountryStateCity(CountryStateCityServiceModel countriesStatesCities) => new ParamListCountryStateCity
        {
            ListCountryStateCityClass = new List<ListCountryStateCityClass>
            {
                new ListCountryStateCityClass
                {
                    CityCd = countriesStatesCities.CityCd,
                    CityDescription = countriesStatesCities.CityDescription,
                    CountryCd = countriesStatesCities.CountryCd,
                    CountryDescription = countriesStatesCities.CountryDescription,
                    StateCd = countriesStatesCities.StateCd,
                    StateDescription = countriesStatesCities.StateDescription
                }
            }
        };

        private static ParamAddressType GetAddressesType(AddressType item) => new ParamAddressType
        {
            AddressTypeList = new List<AddressTypeClass>
            {
                new AddressTypeClass
                {
                    AddressTypeCd = item.Id,
                    Description = item.Description
                }
            }

        };

        private static QuoteParamIdentityCardClass GenerateIdentityCardType(DocumentTypeServiceModel item) => new QuoteParamIdentityCardClass()
        {

            IdentityCardList = new List<IdentityCardTypeClass>
            {
                new IdentityCardTypeClass
                {
                    Description = item.Description,
                    IdentityCardTypeId = item.Id,
                    SmallDescription = item.SmallDescription
                }
            }
        };

        public static List<ParamAssistanceClass> GetListAssistance(List<AssistanceCover> assistanceCovers)
        {
            List<ParamAssistanceClass> paramAssistanceClasses = new List<ParamAssistanceClass>();
            foreach (var item in assistanceCovers)
            {
                paramAssistanceClasses.Add(GetAssistance(item));
            }
            paramAssistanceClasses.Add(new ParamAssistanceClass { ProcessMessage = Errors.ProcessSuccess });

            return paramAssistanceClasses;
        }

        private static ParamAssistanceClass GetAssistance(AssistanceCover item) => new ParamAssistanceClass
        {
            ParamAssistance = new List<ParamAssistance>
            {
                new ParamAssistance
                {
                    AssistanceDescription = item.AssistanceDescription,
                    AssitanceCode = item.AssitanceCode,
                    PrefixCode = item.PrefixCode,
                    PrefixDescription = item.PrefixDescription
                }
            }
        };

        public static List<ParamAssistanceCoverClass> GetListAssistanceCover(List<AssistanceCover> assistanceCovers)
        {
            List<ParamAssistanceCoverClass> paramAssistanceCoverClasses = new List<ParamAssistanceCoverClass>();
            foreach (var item in assistanceCovers)
            {
                paramAssistanceCoverClasses.Add(GetAssistanceCover(item));
            }

            return paramAssistanceCoverClasses;
        }

        private static ParamAssistanceCoverClass GetAssistanceCover(AssistanceCover item) => new ParamAssistanceCoverClass
        {
            ParamAssistanceCover = new ParamAssistanceCover
            {
                AssistanceDescription = item.AssistanceDescription,
                AssitanceCode = item.AssitanceCode,
                PrefixCode = item.PrefixCode,
                PrefixDescription = item.PrefixDescription,
                TextCode = item.TextCode,
                TextDescription = item.TextDescription
            }
        };
    }
}
