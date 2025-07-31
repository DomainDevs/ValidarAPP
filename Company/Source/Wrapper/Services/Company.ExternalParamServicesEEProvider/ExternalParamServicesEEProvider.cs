using Sistran.Company.ExternalParamService;
using Sistran.Company.ExternalParamService.Models;
using SIstran.Company.ExternalParamService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Sistran.Company.ExternalParamServicesEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ExternalParamServicesEEProvider : IExternalParamService
    {
        public List<QuoteParamIdentityCardClass> GenerateIdentityCardTypeList()
        {
            return new List<QuoteParamIdentityCardClass>{new QuoteParamIdentityCardClass
                {
                    IdentityCardList = new List<IdentityCardTypeClass>{
                        new IdentityCardTypeClass
                        {
                            Description = "CEDULA DE CIUDADANIA",
                            IdentityCardTypeId = 1,
                            SmallDescription = "CC"
                        },
                        new IdentityCardTypeClass
                        {
                            Description = "CEDULA DE EXTRANJERIA",
                            IdentityCardTypeId = 2,
                            SmallDescription = "CE"
                        },
                         new IdentityCardTypeClass
                        {
                            Description = "REGISTRO CIVIL",
                            IdentityCardTypeId = 3,
                            SmallDescription = "RC"
                        }
                    }
                }
            };

        }

        public List<ParamAddressType> GenerateListAdressType()
        {
            return new List<ParamAddressType>{new ParamAddressType
               {
                  AddressTypeList = new List<AddressTypeClass>
                  {
                      new AddressTypeClass
                      {
                          AddressTypeCd = 1,
                          Description = "DOMICILIO"
                      },
                      new AddressTypeClass
                      {
                          AddressTypeCd = 2,
                          Description = "OFICINA/COMERCI"
                      },
                      new AddressTypeClass
                      {
                          AddressTypeCd = 3,
                          Description = "CENTRO COMERCIA"
                      }
                  }
               }
           };
        }

        public List<ParamCompanyType> GenerateListCompanyType()
        {
            return new List<ParamCompanyType>
            {
                new ParamCompanyType
                {
                    CompanyTypeClass = new List<CompanyTypeClass>
                    {
                        new CompanyTypeClass
                        {
                            CompanyTypeCd = 1,
                            Description = "ENTIDAD Y ORGANISMO PUBLICO A NIVEL NACIONAL"

                        },
                         new CompanyTypeClass
                        {
                            CompanyTypeCd = 2,
                            Description = "ENTIDAD Y ORGANISMO PUBLICO A NIVEL DEPARTAMENTAL"

                        },
                         new CompanyTypeClass
                        {
                            CompanyTypeCd = 3,
                            Description = "ENTIDAD Y ORGANISMO PUBLICO A NIVEL MUNICIPAL"

                        }
                    }
                }
            };
        }

        public List<ParamListCountryStateCity> GenerateListCountryStateCity()
        {
            return new List<ParamListCountryStateCity>
           {
               new ParamListCountryStateCity
               {
                   ListCountryStateCityClass = new List<ListCountryStateCityClass>
                   {
                       new ListCountryStateCityClass
                       {
                           CityCd = 1,
                           CityDescription = "LA PAZ",
                           CountryCd = 31,
                           CountryDescription = "BOLIVIA",
                           StateCd = 1,
                           StateDescription = "LA PAZ"
                       },
                        new ListCountryStateCityClass
                       {
                           CityCd = 2,
                           CityDescription = "EL ENCANTO",
                           CountryCd = 1,
                           CountryDescription = "COLOMBIA",
                           StateCd = 30,
                           StateDescription = "AMAZONAS"
                       },
                         new ListCountryStateCityClass
                       {
                           CityCd = 4,
                           CityDescription = "LA PEDRERA",
                           CountryCd = 1,
                           CountryDescription = "COLOMBIA",
                           StateCd = 30,
                           StateDescription = "AMAZONAS"
                       }
                   }
               }
           };
        }

        public List<ParamCompanyType> GenerateListDetail()
        {
            return new List<ParamCompanyType>
           {
               new ParamCompanyType
               {
                   CompanyTypeClass = new List<CompanyTypeClass>
                   {
                       new CompanyTypeClass
                       {
                           CompanyTypeCd = 1,
                           Description = "EQUALIZADOR"
                       },
                        new CompanyTypeClass
                       {
                           CompanyTypeCd = 2,
                           Description = "BOCINA"
                       }
                   }
               }
           };
        }

        public List<ParamEconomicActivity> GenerateListEconomicActivity()
        {
            return new List<ParamEconomicActivity>
           {
               new ParamEconomicActivity
               {
                   EconomicActivityClass = new List<EconomicActivityClass>
                   {
                       new EconomicActivityClass
                       {
                          Description = "Asalariados",
                          EconomicActivityCd = 10
                       },
                        new EconomicActivityClass
                       {
                          Description = "Independiente",
                          EconomicActivityCd = 11
                       },
                         new EconomicActivityClass
                       {
                          Description = "Rentistas de capital. solo para personas naturales",
                          EconomicActivityCd = 90
                       }
                   }
               }
           };
        }

        public List<ParamListProduct> GenerateListProduct(int prefixCd)
        {
            if (prefixCd > 0)
            {
                return new List<ParamListProduct>
               {
                   new ParamListProduct
                   {
                       ListProductClass = new List<ListProductClass>
                       {
                           new ListProductClass
                           {
                              PrefixCd = 30,
                              DescriptionPrefix = "CUMPLIMIENTO",
                              ProductCd = 135,
                              DescriptionProduct = "CU Seriedad de Oferta Particulares",
                              GroupCoverageCd = 1,
                              DescriptionGroup = "Unico"
                           },
                            new ListProductClass
                           {
                              PrefixCd = 30,
                              DescriptionPrefix = "CUMPLIMIENTO",
                              ProductCd = 136,
                              DescriptionProduct = "CU Cumplimiento Particulares",
                              GroupCoverageCd = 1,
                              DescriptionGroup = "Unico"
                           },
                             new ListProductClass
                           {
                              PrefixCd = 30,
                              DescriptionPrefix = "CUMPLIMIENTO",
                              ProductCd = 138,
                              DescriptionProduct = "CU Seriedad Grandes Beneficiarios",
                              GroupCoverageCd = 1,
                              DescriptionGroup = "Unico"
                           }
                       }
                   }
               };
            }
            else
            {
                return new List<ParamListProduct>
                {
                    new ParamListProduct
                    {
                      ProcessMessage = "La cadena de entrada no tiene el formato correcto"
                    }
                };
            }
        }

        public List<ParamListRestrictive> GenerateListRestrictive()
        {
            return new List<ParamListRestrictive>
           {
              new ParamListRestrictive
              {
                 ListRestrictiveClass = new List<ListRestrictiveClass>
                 {
                    new ListRestrictiveClass
                    {
                        DocumetNum = "1",
                        DocumetType ="10000791"
                    },
                     new ListRestrictiveClass
                    {
                        DocumetNum = "P",
                        DocumetType ="100009"
                    },
                      new ListRestrictiveClass
                    {
                        DocumetNum = "C",
                        DocumetType ="10001461"
                    }
                 }
              }
           };
        }

        public List<QuoteParamRatingZoneClass> GenerateRatingZoneList()
        {
            return new List<QuoteParamRatingZoneClass>
            {
                new QuoteParamRatingZoneClass
                {
                    RatingZoneClass = new List<RatingZoneClass>
                    {
                        new RatingZoneClass
                        {
                            RatingZoneCode = 1,
                            Description = "C/MARCA",
                            SmallDescription = "C/MARCA-AUTOMOV",
                            PrefixCode = 10,
                            IsDefault = false
                        },
                         new RatingZoneClass
                        {
                            RatingZoneCode = 2,
                            Description = "SANTANDER",
                            SmallDescription = "S/TANDER-AUTOMO",
                            PrefixCode = 10,
                            IsDefault = false
                        },
                          new RatingZoneClass
                        {
                            RatingZoneCode = 3,
                            Description = "EJE CAFETERO",
                            SmallDescription = "EJE CAFETERO-AU",
                            PrefixCode = 10,
                            IsDefault = false
                        }
                    }
                }
            };
        }

        public QuoteParamVehicleFasecoldaClass GenerateVehicleFasecolda(string fasecoldaCd, int yearVehicle)
        {
            if (fasecoldaCd != null)
            {
                return new QuoteParamVehicleFasecoldaClass
                {
                    VehicleFasecolda = new ExternalParamService.DTO.VehicleParameterDTO
                    {
                        VehicleMakeCode = 1,
                        VehicleModelCode = 1,
                        VehicleVersionCode = 1,
                        VehicleTypeCode = 1,
                        MakeDescription = "ALEKO",
                        ModelDescription = "2141",
                        VersionDescription = "1.6 MT 1600CC TAXI",
                        VehicleTypeDescription = "AUTOMOVIL",
                        VehicleYear = 2018,
                        VehiclePrice = 12200000,
                        FasecoldaMakeId = "001",
                        FasecoldaModelId = "01001"
                    }
                };
            }
            else
            {
                return new QuoteParamVehicleFasecoldaClass
                {
                    ProcessMessage = "La cadena de entrada no tiene el formato correcto."
                };
            }
        }

        public List<QuoteParamVehicleClass> GenerateVehiclesList(int vehicleYearInit, int vehicleYearEnd)
        {

            if (vehicleYearInit > 0 && vehicleYearEnd > 0)

            {
                return new List<QuoteParamVehicleClass>
               {
                  new QuoteParamVehicleClass
                  {
                       VehicleParameterDTOCo = new List<ExternalParamService.DTO.VehicleParameterDTO>
                       {
                           new ExternalParamService.DTO.VehicleParameterDTO
                           {
                                VehicleMakeCode = 1,
                                VehicleModelCode=1,
                                VehicleVersionCode=1,
                                VehicleTypeCode=1,
                                MakeDescription="ALEKO",
                                ModelDescription="2141",
                                VersionDescription="1.6 MT 1600CC TAXI",
                                VehicleTypeDescription="AUTOMOVIL",
                                VehicleYear=2018,
                                VehiclePrice=12200000,
                                FasecoldaMakeId="001",
                                FasecoldaModelId="01001"
                           },
                             new ExternalParamService.DTO.VehicleParameterDTO
                           {
                                VehicleMakeCode = 2,
                                VehicleModelCode=2,
                                VehicleVersionCode=3,
                                VehicleTypeCode=6,
                                MakeDescription="AMERICAN MOTOR",
                                ModelDescription="EAGLE",
                                VersionDescription="SUMMIT AT 2400CC LX 4P",
                                VehicleTypeDescription="CAMIONETA PASAJEROS",
                                VehicleYear=2018,
                                VehiclePrice=43800000,
                                FasecoldaMakeId="002",
                                FasecoldaModelId="06001"
                           }
                       }
                  }
               };
            }
            else
            {
                return new List<QuoteParamVehicleClass>
                {
                    new QuoteParamVehicleClass
                    {
                       ProcessMessage = "La cadena de entrada no tiene el formato correcto."
                    }
                };
            }
        }

        public List<PhoneTypeClass> GenereteListPhoneType()
        {
            return new List<PhoneTypeClass>
            {
               new PhoneTypeClass
               {
                   Description = "DOMICILIO",
                   PhoneTypeCd = 1
               },
                new PhoneTypeClass
               {
                   Description = "OFICINA/COMERCIAL",
                   PhoneTypeCd = 2
               },
                 new PhoneTypeClass
               {
                   Description = "TELEFAX",
                   PhoneTypeCd = 3
               }
            };
        }

        public AgentModel GetAgent(int agentCode) => new AgentModel
        {
            IndividualTypeCd = 1,
            Name = "FISCALIA",
            Surname = "DE LA NACION",
            MotherLastName = "FISCALIA",
            TradeName = "FISCALIA"
        };

        public List<ParamAssistanceClass> GetListAssistance(int prefixCd, int productCd)
        {
            return new List<ParamAssistanceClass>
           {
               new ParamAssistanceClass
               {
                   ParamAssistance = new List<ParamAssistance>
                   {
                       new ParamAssistance
                       {
                           PrefixDescription = "CUMPLIMIENTO",
                           PrefixCode = 30,
                           AssistanceDescription ="",
                           AssitanceCode = 1

                       }
                   }
               }
           };
        }

        public List<ParamAssistanceCoverClass> GetListAssistanceCover(int assistanceCd, int prefixCd)
        {
            return new List<ParamAssistanceCoverClass>
           {
               new ParamAssistanceCoverClass
               {
                   ParamAssistanceCover = new ParamAssistanceCover
                   {
                       AssistanceDescription = "",
                       AssitanceCode = 1,
                       PrefixCode = 30,
                       PrefixDescription = "CUMPLIMIENTO",
                       TextCode = 20,
                       TextDescription = "CUMPLIMIENTO"
                   }
               }
           };
        }

        public LastDateClass GetModifyInfo()
        {
            return new LastDateClass
            {
                UpDate = new DateTime(2018 - 05 - 02)
            };
        }

        public ProductLimitAmtModel GetProductLimitAmt(int productId)
        {
            if (productId > 0)
            {
                return new ProductLimitAmtModel
                {
                    LimitAmt = 5655,
                    ProductId = productId
                };
            }
            else
            {
                return new ProductLimitAmtModel
                {
                    Message = "No tiene un Limite por Producto"
                };
            }
        }
    }
}
