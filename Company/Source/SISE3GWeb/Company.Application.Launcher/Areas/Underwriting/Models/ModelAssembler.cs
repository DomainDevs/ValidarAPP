using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using CLOCATIONMODEL = Sistran.Company.Application.Locations.Models;
using CSUM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using CTPLM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using ISSMODEL = Sistran.Core.Application.UnderwritingServices.Models;
using LBTY = Sistran.Company.Application.Location.LiabilityServices.Models;
using LOCATIONMODEL = Sistran.Core.Application.Locations.Models;
using PROP = Sistran.Company.Application.Location.PropertyServices.Models;
using VECOM = Sistran.Company.Application.Vehicles.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    using AutoMapper;
    using Newtonsoft.Json.Linq;
    using Sistran.Company.Application.CommonServices.Models;
    using Sistran.Company.Application.Finances.FidelityServices.Models;
    using Sistran.Company.Application.Locations.Models;
    using Sistran.Company.Application.ProductServices.Models;
    using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
    using Sistran.Company.Application.Sureties.SuretyServices.Models;
    using Sistran.Company.Application.UnderwritingServices;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Company.Application.Vehicles.Models;
    using Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.ProductServices.Models;
    using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using System.Globalization;
    using System.Linq;
    using UWM = Sistran.Company.Application.UnderwritingServices.Models;

    public class ModelAssembler
    {
        #region Policy

        public static CompanyPolicy CreatePolicy(PolicyModelsView policyModel)
        {
            CompanyPolicy policy = new CompanyPolicy
            {
                Summary = new CompanySummary
                {
                    RiskCount = policyModel.RisksQuantity
                },
                IssueDate = policyModel.IssueDate,
                Holder = new ISSMODEL.Holder
                {

                    IndividualId = policyModel.HolderId,
                    IndividualType = (IndividualType)policyModel.HolderIndividualType.GetValueOrDefault(),
                    Name = policyModel.HolderName,
                    CustomerType = (CustomerType)policyModel.HolderCustomerType,
                    PaymentMethod = new ISSMODEL.IssuancePaymentMethod
                    {
                        Id = policyModel.HolderPaymentMethodId,
                        PaymentId = policyModel.HolderPaymentIdId
                    },
                    BirthDate = policyModel.HolderBirthDate.GetValueOrDefault(),
                    Gender = policyModel.HolderGender,
                    InsuredId = policyModel.HolderInsuredId,
                    DeclinedDate = policyModel.HolderDeclinedDate.GetValueOrDefault(),
                    CompanyName = new ISSMODEL.IssuanceCompanyName
                    {
                        NameNum = policyModel.HolderDetailId.GetValueOrDefault(),
                        Address = new ISSMODEL.IssuanceAddress
                        {
                            Id = policyModel.HolderAddressId,
                            City = new City
                            {
                                State = new State
                                {
                                    Id = policyModel.HolderStateId,
                                    Country = new Country
                                    {
                                        Id = policyModel.HolderCountryId
                                    }
                                }
                            }
                        }
                    },
                    IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                    {
                        Number = policyModel.HolderIdentificationDocument,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = policyModel?.HolderDocumentType == null ? 0 : (int)policyModel?.HolderDocumentType,

                        }
                    }

                },

                Branch = new CompanyBranch
                {
                    Id = policyModel.BranchId,
                    SalePoints = policyModel.SalePoint.HasValue ? new List<CompanySalesPoint> {
                        new CompanySalesPoint{
                            Id = policyModel.SalePoint.Value,
                        }
                    } : null
                },
                Prefix = new CompanyPrefix
                {
                    Id = policyModel.PrefixId,
                    Description = policyModel.PrefixName
                },
                Product = new CompanyProduct
                {
                    Id = policyModel.ProductId,
                    Description = policyModel.ProductName
                },
                PolicyType = new CompanyPolicyType
                {
                    Id = policyModel.PolicyType
                },
                CurrentFrom = policyModel.CurrentFrom,
                TimeHour = policyModel.TimeHour,
                CurrentTo = policyModel.CurrentTo,
                ExchangeRate = new ExchangeRate
                {
                    Currency = new Currency
                    {
                        Id = policyModel.Currency
                    },
                    SellAmount = Convert.ToDecimal(policyModel.ExchangeRate),
                    RateDate = DateTime.MinValue
                },
                TemporalType = (TemporalType)policyModel.TemporalType,
                Endorsement = new CompanyEndorsement
                {
                    EndorsementType = (EndorsementType)policyModel.EndorsementType,
                    QuotationId = policyModel.QuotationId.GetValueOrDefault(),
                    QuotationVersion = policyModel.QuotationVersion.GetValueOrDefault()
                },
                Agencies = new List<ISSMODEL.IssuanceAgency>()
            };

            if (policyModel.Id.HasValue)
            {
                policy.Id = policyModel.Id.Value;
            }

            if (policyModel.HolderPhoneId.GetValueOrDefault() > 0)
            {
                policy.Holder.CompanyName.Phone = new ISSMODEL.IssuancePhone
                {
                    Id = policyModel.HolderPhoneId.Value
                };
            }

            if (policyModel.HolderEmailId.GetValueOrDefault() > 0)
            {
                policy.Holder.CompanyName.Email = new ISSMODEL.IssuanceEmail
                {
                    Id = policyModel.HolderEmailId.Value
                };
            }

            if (policyModel.Request.HasValue)
            {
                policy.Request = new ISSMODEL.Request
                {
                    Id = policyModel.Request.Value
                };
            }
            if (policyModel.BillingGroup.HasValue)
            {
                policy.BillingGroup = new CompanyBillingGroup
                {
                    Id = policyModel.BillingGroup.Value
                };
            }

            if (policy.TemporalType == TemporalType.Quotation)
            {
                policy.TemporalTypeDescription = "Cotización";
            }
            else
            {
                policy.Endorsement.TicketDate = policyModel.TicketDate;
                policy.Endorsement.TicketNumber = policyModel.TicketNumber;
                policy.TemporalTypeDescription = "Temporal";
            }

            policy.Agencies.Add(new ISSMODEL.IssuanceAgency
            {
                Id = policyModel.AgencyId,
                Code = policyModel.AgentCode,
                FullName = policyModel.AgencyName,
                IsPrincipal = true,
                Agent = new ISSMODEL.IssuanceAgent
                {
                    IndividualId = policyModel.AgentId,
                    FullName = policyModel.AgentName
                },
                Branch = new Branch
                {
                    Id = policyModel.AgencyBranchId
                },
                Commissions = new List<ISSMODEL.IssuanceCommission>()
            });

            policy.Endorsement.TicketDate = policyModel.TicketDate;
            policy.Endorsement.TicketNumber = policyModel.TicketNumber;
            policy.JustificationSarlaft = new CompanyJustificationSarlaft()
            {
                JustificationReasonCode = (int)policyModel.JustificationSarlaft
            };
            policy.CalculateMinPremium = policyModel.CalculateMinPremium == null ? false : policyModel.CalculateMinPremium;
            policy.PolicyOrigin = PolicyOrigin.Individual;
            policy.AppSourceR2 = true;
            return policy;
        }

        public static CompanyPolicy CompanyCreatePolicy(PolicyModelsView policyModel)
        {
            if (policyModel.TemporalType == (int)TemporalType.TempQuotation)
            {
                policyModel.QuotationVersion = 0;
            }
            CompanyPolicy policy = new CompanyPolicy
            {
                IsPersisted = true,
                Summary = new CompanySummary
                {
                    RiskCount = policyModel.RisksQuantity
                },
                IssueDate = policyModel.IssueDate,
                Holder = new Holder
                {

                    IndividualId = policyModel.HolderId,
                    IndividualType = (IndividualType)policyModel.HolderIndividualType.GetValueOrDefault(),
                    Name = policyModel.HolderName,
                    CustomerType = (CustomerType)policyModel.HolderCustomerType,
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        Id = policyModel.HolderPaymentMethodId,
                        PaymentId = policyModel.HolderPaymentIdId
                    },
                    BirthDate = policyModel.HolderBirthDate.GetValueOrDefault(),
                    Gender = policyModel.HolderGender,
                    InsuredId = policyModel.HolderInsuredId,
                    DeclinedDate = policyModel.HolderDeclinedDate.GetValueOrDefault(),
                    CompanyName = new IssuanceCompanyName
                    {
                        NameNum = policyModel.HolderDetailId.GetValueOrDefault(),
                        Address = new IssuanceAddress
                        {
                            Id = policyModel.HolderAddressId,
                            City = new City
                            {
                                State = new State
                                {
                                    Id = policyModel.HolderStateId,
                                    Country = new Country
                                    {
                                        Id = policyModel.HolderCountryId
                                    }
                                }
                            }
                        }
                    },
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = policyModel.HolderIdentificationDocument,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = policyModel?.HolderDocumentType == null ? 0 : (int)policyModel.HolderDocumentType,
                        }
                    },

                    AssociationType =new IssuanceAssociationType
                    {
                        Id = policyModel.HolderAssociationType
                    }

                },

                Branch = new CompanyBranch
                {
                    Id = policyModel.BranchId,
                    Description = policyModel.BranchName,
                    SalePoints = policyModel.SalePoint.HasValue ? new List<CompanySalesPoint> {
                        new CompanySalesPoint{
                            Id = policyModel.SalePoint.Value,
                        }
                    } : null
                },
                Prefix = new CompanyPrefix
                {
                    Id = policyModel.PrefixId,
                    Description = policyModel.PrefixName
                },
                Product = new CompanyProduct
                {
                    Id = policyModel.ProductId,
                    Description = policyModel.ProductName
                },
                PolicyType = new CompanyPolicyType
                {
                    Id = policyModel.PolicyType,
                    IsFloating = Convert.ToBoolean(policyModel.IsFloating)

                },
                BusinessType = policyModel.BusinessType,
                CurrentFrom = policyModel.CurrentFrom,
                TimeHour = policyModel.TimeHour,
                CurrentTo = policyModel.CurrentTo,
                ExchangeRate = new ExchangeRate
                {
                    Currency = new Currency
                    {
                        Id = policyModel.Currency
                    },
                    SellAmount = Convert.ToDecimal(policyModel.ExchangeRate),
                    RateDate = DateTime.MinValue
                },
                TemporalType = (TemporalType)policyModel.TemporalType,

                Endorsement = new CompanyEndorsement
                {
                    EndorsementType = (EndorsementType)policyModel.EndorsementType,
                    QuotationId = policyModel.QuotationId.GetValueOrDefault(),

                    QuotationVersion = policyModel.QuotationVersion.GetValueOrDefault()
                },
                Agencies = new List<IssuanceAgency>()
            };

            if (policyModel.Id.HasValue)
            {
                policy.Id = policyModel.Id.Value;
            }

            if (policyModel.HolderPhoneId.GetValueOrDefault() > 0)
            {
                policy.Holder.CompanyName.Phone = new IssuancePhone
                {
                    Id = policyModel.HolderPhoneId.Value
                };
            }

            if (policyModel.HolderEmailId.GetValueOrDefault() > 0)
            {
                policy.Holder.CompanyName.Email = new IssuanceEmail
                {
                    Id = policyModel.HolderEmailId.Value
                };
            }

            if (policyModel.Request.HasValue)
            {
                policy.Request = new ISSMODEL.Request
                {
                    Id = policyModel.Request.Value
                };
            }
            if (policyModel.BillingGroup.HasValue)
            {
                policy.BillingGroup = new CompanyBillingGroup
                {
                    Id = policyModel.BillingGroup.Value
                };
            }

            if (policy.TemporalType == TemporalType.Quotation)
            {
                policy.TemporalTypeDescription = "Cotización";
            }
            else
            {
                policy.TemporalTypeDescription = "Temporal";
                policy.Endorsement.TicketDate = policyModel.TicketDate;
                policy.Endorsement.TicketNumber = policyModel.TicketNumber;
            }

            policy.Agencies.Add(new IssuanceAgency
            {
                Id = policyModel.AgencyId,
                Code = policyModel.AgentCode,
                FullName = policyModel.AgencyName,
                IsPrincipal = true,
                Agent = new IssuanceAgent
                {
                    IndividualId = policyModel.AgentId,
                    FullName = policyModel.AgentName,
                    AgentType = new IssuanceAgentType { Id = policyModel.AgentType }
                },
                Branch = new Branch
                {
                    Id = policyModel.AgencyBranchId,
                    Description = policyModel.AgencyBranchDesc,
                    SalePoints = new List<SalePoint>()
                    {
                        new SalePoint()
                        {
                            Id = policyModel.AgencyBranchSalePointId,
                            Description = policyModel.AgencyBranchSalePointDesc
                        }
                    }
                },
                Commissions = new List<IssuanceCommission>()
            });
            if (policyModel.JustificationSarlaft != null)
            {
                policy.JustificationSarlaft = new CompanyJustificationSarlaft()
                {
                    JustificationReasonCode = (int)policyModel.JustificationSarlaft
                };
            }

            policy.Endorsement.TicketDate = policyModel.TicketDate;
            policy.Endorsement.TicketNumber = policyModel.TicketNumber;
            policy.CalculateMinPremium = policyModel.CalculateMinPremium == null ? false : policyModel.CalculateMinPremium;
            policy.UserId = SessionHelper.GetUserId();
            policy.PolicyOrigin = PolicyOrigin.Individual;
            policy.AppSourceR2 = true;
            return policy;
        }

        #endregion

        #region Commission

        public static IssuanceAgency CreateAgency(AgentModelsView agentModel)
        {
            return new IssuanceAgency
            {
                Id = agentModel.AgencyId,
                Code = agentModel.AgencyCode,
                FullName = agentModel.AgencyName,
                IsPrincipal = agentModel.IsPrincipal,
                Participation = agentModel.Participation,
                Agent = new IssuanceAgent
                {
                    IndividualId = agentModel.AgentId,
                    FullName = agentModel.AgentName
                },
                Branch = new Branch
                {
                    Id = agentModel.AgencyBranchId
                },
                Commissions = new List<IssuanceCommission>()
            };
        }

        public static IssuanceCommission CreateCommission(AgentModelsView agentModel)
        {
            IssuanceCommission commission = new IssuanceCommission
            {
                Percentage = agentModel.Percentage,
                PercentageAdditional = agentModel.PercentageAdditional.GetValueOrDefault(0)
            };

            return commission;
        }

        public static List<IssuanceAgency> CreateAgencies(List<AgentModelsView> agenciesModel)
        {
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();

            foreach (AgentModelsView item in agenciesModel)
            {
                IssuanceAgency agency = ModelAssembler.CreateAgency(item);
                agency.Commissions = new List<IssuanceCommission>();
                agency.Commissions.Add(ModelAssembler.CreateCommission(item));
                agencies.Add(agency);
            }

            return agencies;
        }

        #endregion

        #region Text

        public static CompanyText CreateText(TextsModelsView textModel)
        {
            CompanyText text = new CompanyText
            {
                TextBody = Regex.Replace(textModel?.TextBody ?? "", @"[/']", " ", RegexOptions.None),
                Observations = textModel.Observations
            };

            return text;
        }

        #endregion

        #region CoInsurance

        public static CompanyIssuanceCoInsuranceCompany CreateAcceptedCoInsuranceCompany(CoInsuranceModelsView coInsuranceModel)
        {
            CompanyIssuanceCoInsuranceCompany coInsuranceCompany = new CompanyIssuanceCoInsuranceCompany
            {
                Id = coInsuranceModel.AcceptedCoinsurerId.Value,
                Description = coInsuranceModel.AcceptedCoinsurerName,
                ParticipationPercentageOwn = coInsuranceModel.AcceptedParticipationPercentageOwn.Value,
                ParticipationPercentage = coInsuranceModel.AcceptedParticipationPercentage.Value,
                ExpensesPercentage = coInsuranceModel.AcceptedExpensesPercentage.Value,
                PolicyNumber = coInsuranceModel.AcceptedPolicyNumber,
                EndorsementNumber = coInsuranceModel.AcceptedEndorsementNumber


            };
            return coInsuranceCompany;
        }

        #endregion

        #region VehicleRisk

        public static CompanyVehicle CreateVehicle(RiskVehicleModelsView riskModel, AdditionalDataModelsView additionalData)
        {
            CompanyVehicle vehicle = new CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    Id = riskModel.RiskId.GetValueOrDefault(),
                    CoveredRiskType = CoveredRiskType.Vehicle,
                    Description = riskModel.LicensePlate == null ? "" : riskModel.LicensePlate,
                    RiskId = riskModel.OriginalRiskId.GetValueOrDefault(),
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId,
                        IndividualType = IndividualType.Person,
                        Name = riskModel.InsuredName,
                        CustomerType = (CustomerType)riskModel.InsuredCustomerType,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            Number = riskModel.InsuredDocumentNumber
                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = riskModel.InsuredDetailId.GetValueOrDefault(),
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = riskModel.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = riskModel.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = riskModel.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        BirthDate = riskModel.InsuredBirthDate.GetValueOrDefault(),
                        Gender = riskModel.InsuredGender
                    },
                    RatingZone = new CompanyRatingZone
                    {
                        Id = riskModel.RatingZone
                    },
                    GroupCoverage = new ISSMODEL.GroupCoverage
                    {
                        Id = riskModel.GroupCoverage
                    },
                    LimitRc = new CompanyLimitRc
                    {
                        Id = riskModel.LimitRC
                    },
                    Status = RiskStatusType.Original,
                    IsPersisted = true
                },
                Make = new VECOM.CompanyMake
                {
                    Id = riskModel.Make,
                    Description = riskModel.MakeDescription
                },
                Model = new VECOM.CompanyModel
                {
                    Id = riskModel.Model,
                    Description = riskModel.ModelDescription
                },
                Version = new VECOM.CompanyVersion
                {
                    Id = riskModel.Version,
                    Type = new VECOM.CompanyType
                    {
                        Id = riskModel.Type
                    },
                    Description = riskModel.VersionDescription,
                    Fuel = new VECOM.CompanyFuel
                    {
                        Id = additionalData.FuelType
                    },
                    Body = new VECOM.CompanyBody
                    {
                        Id = additionalData.BodyType
                    }
                },
                Fasecolda = new Fasecolda
                {
                    Description = riskModel.FasecoldaCode,
                },
                Use = new CompanyUse
                {
                    Id = riskModel.Use
                },
                Year = riskModel.Year,
                IsNew = riskModel.IsNew,
                EngineSerial = riskModel.Engine == null ? "" : riskModel.Engine,
                ChassisSerial = riskModel.Chassis == null ? "" : riskModel.Chassis,
                Color = new VECOM.CompanyColor
                {
                    Id = riskModel.Color
                },

                Price = Convert.ToDecimal(riskModel.Price),
                StandardVehiclePrice = Convert.ToDecimal(riskModel.StandardVehiclePrice),
                PriceAccesories = Convert.ToDecimal(riskModel.PriceAccesories),

                Rate = Convert.ToDecimal(riskModel.Rate),

                OriginalPrice = riskModel.OriginalPrice,
                //ServiceType = new VECOM.CompanyServiceType
                //{
                //    Id = (int)enumWeb.AutoServiceType.Particular
                //},
                NewPrice = Convert.ToDecimal(additionalData.NewPrice),
                LicensePlate = riskModel.LicensePlate,
                ServiceType = new CompanyServiceType()
                {
                    Id = riskModel.ServiceType == null ? 0 : (int)riskModel.ServiceType,
                    Description = riskModel.ServiceTypeDescription
                }




            };

            if (!string.IsNullOrEmpty(riskModel.AmountInsured))
            {
                vehicle.Risk.AmountInsured = Convert.ToDecimal(riskModel.AmountInsured);
            }

            if (!string.IsNullOrEmpty(riskModel.Premium))
            {
                vehicle.Risk.Premium = Convert.ToDecimal(riskModel.Premium);
            }

            return vehicle;
        }

        #endregion

        #region CompanyPolicy
        public static List<CompanyPolicyAgent> CreateAgenciesCompany(List<ProductAgency> agencies)
        {
            List<CompanyPolicyAgent> companyPolicyAgents = new List<CompanyPolicyAgent>();
            foreach (var item in agencies)
            {
                companyPolicyAgents.Add(CreateOneAgencieCompany(item));
            }
            return companyPolicyAgents;
        }

        public static CompanyPolicyAgent CreateOneAgencieCompany(ProductAgency agency)
        {
            CompanyPolicyAgent companyAgency = new CompanyPolicyAgent();
            companyAgency.AgentId = agency.Code;
            companyAgency.IndividualId = agency.Agent.IndividualId;
            companyAgency.Id = agency.Id;
            companyAgency.FullName = agency.FullName;

            return companyAgency;
        }
        #endregion

        #region PropertyRisk

        public static CompanyPropertyRisk CreatePropertyRisk(RiskPropertyViewModel riskModel)
        {
            CompanyPropertyRisk risk = new CompanyPropertyRisk
            {
                Risk = new CompanyRisk
                {
                    Id = riskModel?.RiskId == null ? 0 : (int)riskModel.RiskId,
                    Description = riskModel.FullAddress,
                    Policy = new CompanyPolicy
                    {
                        Id = riskModel.TemporalId,
                        Product = new CompanyProduct { Id = riskModel.ProductId },
                        Prefix = new CompanyPrefix { Id = riskModel.PrefixId },
                    },
                    RiskActivity = new CompanyRiskActivity
                    {
                        Id = riskModel.RiskActivityId.GetValueOrDefault(),
                        Description = riskModel.RiskActivityDescription
                    },
                    GroupCoverage = new ISSMODEL.GroupCoverage
                    {
                        Id = riskModel.GroupCoverage
                    },
                    Status = RiskStatusType.Original,
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId,
                        Name = riskModel.InsuredName,
                        CustomerType = (CustomerType)riskModel.InsuredCustomerType,
                        IndividualType = IndividualType.Person,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            Number = riskModel.InsuredDocumentNumber
                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = riskModel.InsuredDetailId.GetValueOrDefault(),
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = riskModel.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = riskModel.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = riskModel.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        BirthDate = riskModel.InsuredBirthDate,
                        Gender = riskModel.InsuredGender
                    },
                    SecondInsured = new CompanyIssuanceInsured
                    {
                        InsuredId = riskModel.InsuredId,
                        Name = riskModel.InsuredName,
                        IndividualType = IndividualType.Person,
                        CustomerType = CustomerType.Individual,
                    }
                },

                HasNomenclature = riskModel.HasNomenclature,
                NomenclatureAddress = new CompanyNomenclatureAddress

                {

                    Type = new CompanyRouteType
                    {
                        Id = riskModel.RouteTypeId
                    },

                    Name = riskModel.Name,
                    Letter = riskModel.Letter,
                    Suffix1 = new LOCATIONMODEL.Suffix
                    {
                        Id = riskModel.Suffix1Id.GetValueOrDefault()
                    },
                    Number1 = riskModel.Number1.GetValueOrDefault(),
                    Letter2 = riskModel.Letter2,
                    Number2 = riskModel.Number2.GetValueOrDefault(),
                    Suffix2 = new LOCATIONMODEL.Suffix
                    {
                        Id = riskModel.Suffix2Id.GetValueOrDefault()
                    },
                    ApartmentOrOffice = new CompanyApartmentOrOffice
                    {
                        Id = riskModel.ApartmentOrOfficeId
                    },
                    Number3 = riskModel.Number3.GetValueOrDefault(),
                },
                FullAddress = riskModel.FullAddress,
                //Description = riskModel.FullAddress,
                City = new City
                {
                    Id = riskModel.CityCode,
                    DANECode = riskModel.DaneCode,
                    State = new State
                    {
                        Id = riskModel.StateCode,
                        Country = new Country
                        {
                            Id = riskModel.CountryCode
                        }
                    }
                },
                IsDeclarative = riskModel.IsDeclarative

            };
            if (risk.RiskType == null)
            {
                risk.RiskType = new RiskType
                {
                    Id = riskModel.RiskType != null ? riskModel.RiskType.Value : 0
                };
            }
            if (riskModel.RiskId.HasValue)
            {
                risk.Risk.Id = riskModel.RiskId.Value;
            }
            if (!string.IsNullOrEmpty(riskModel.AmountInsured))
                if (!string.IsNullOrEmpty(riskModel.Premium))
                {
                    risk.Risk.Premium = Convert.ToDecimal(riskModel.Premium);
                }

            if (riskModel.RateZoneDescription != null)
            {
                risk.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = riskModel.RateZoneId,
                    Description = riskModel.RateZoneDescription
                };
            }
            if (riskModel.AdditionalDataViewModel != null)
            {
                risk.PML = riskModel.AdditionalDataViewModel.PML;
                risk.Square = riskModel.AdditionalDataViewModel.Square;
                risk.ConstructionYear = riskModel.AdditionalDataViewModel.ConstructionYear;

                risk.FloorNumber = riskModel.AdditionalDataViewModel.FloorNumber;
                risk.Latitude = riskModel.AdditionalDataViewModel.Latitude;
                risk.Longitude = riskModel.AdditionalDataViewModel.Longitude;
                risk.RiskAge = riskModel.AdditionalDataViewModel.RiskAge;

                if (riskModel.AdditionalDataViewModel.ConstructionType > 0)
                {
                    risk.ConstructionType = new CompanyConstructionType
                    {
                        Id = riskModel.AdditionalDataViewModel.ConstructionType
                    };
                }
                if (riskModel.AdditionalDataViewModel.RiskType > 0)
                {
                    risk.RiskType = new RiskType
                    {
                        Id = riskModel.AdditionalDataViewModel.RiskType
                    };
                }
                if (riskModel.AdditionalDataViewModel.RiskUse > 0)
                {
                    risk.RiskUse = new CompanyRiskUse
                    {
                        Id = riskModel.AdditionalDataViewModel.RiskUse
                    };
                }

                if (riskModel.AdditionalDataViewModel.BillingPeriodDepositPremium > 0)
                {
                    foreach (CompanyCoverage item in risk.Risk.Coverages)
                    {
                        item.InsuredObject.BillingPeriodDepositPremium = riskModel.AdditionalDataViewModel.BillingPeriodDepositPremium;
                    }
                }

                if (riskModel.AdditionalDataViewModel.AdjustPeriod != null)
                {
                    foreach (CompanyCoverage item in risk.Risk.Coverages)
                    {
                        item.InsuredObject.BillingPeriodDepositPremium = riskModel.AdditionalDataViewModel.BillingPeriodDepositPremium;
                    }
                }

                if (riskModel.AdditionalDataViewModel.DeclarationPeriod != null)
                {
                    foreach (CompanyCoverage item in risk.Risk.Coverages)
                    {
                        item.InsuredObject.DeclarationPeriod = Convert.ToInt16(riskModel.AdditionalDataViewModel.DeclarationPeriod);
                    }

                }

                risk.PML = riskModel.AdditionalDataViewModel.PML;
                risk.Square = riskModel.AdditionalDataViewModel.Square;

                if (riskModel.RateZoneDescription != null)
                {
                    risk.Risk.RatingZone = new CompanyRatingZone { Id = riskModel.RateZoneId, Description = riskModel.RateZoneDescription };
                }

                risk.Risk.IsRetention = riskModel.IsRetention;
                risk.Risk.IsFacultative = riskModel.IsFacultative;

                if (riskModel.RiskSubActivityId != 0)
                {
                    risk.RiskSubActivity = new PROP.CompanyRiskSubActivity
                    {
                        Id = (int)riskModel.RiskSubActivityId,
                        Description = riskModel.RiskSubActivityDescripcion
                    };
                }
                risk.PrincipalRisk = riskModel.AdditionalDataViewModel.PrincipalRisk;
                if (riskModel.AssuranceModeId != 0)
                    risk.AssuranceMode = new PROP.CompanyAssuranceMode
                    {
                        Id = (int)riskModel.AssuranceModeId,
                        Description = riskModel.AssuranceModeDescripcion
                    };

            }
            return risk;
        }

        public static RiskPropertyViewModel CreateProperty(CompanyPropertyRisk risk)
        {
            RiskPropertyViewModel riskModel = new RiskPropertyViewModel
            {

                RiskId = risk.Risk.Id,
                GroupCoverage = risk.Risk.GroupCoverage == null ? 0 : risk.Risk.GroupCoverage.Id,
                AmountInsured = risk.Risk.AmountInsured.ToString(),
                Premium = risk.Risk.Premium.ToString(),
                InsuredId = risk.Risk.MainInsured == null ? 0 : risk.Risk.MainInsured.InsuredId,
                InsuredName = risk.Risk.MainInsured == null ? "" : risk.Risk.MainInsured.Name,
                HasNomenclature = risk.HasNomenclature,
                FullAddress = risk.FullAddress,
                RateZoneDescription = risk.Risk.RatingZone == null ? "" : risk.Risk.RatingZone.Description,
                RateZoneId = risk.Risk.RatingZone == null ? 0 : risk.Risk.RatingZone.Id
            };

            if (risk.City != null)
            {
                riskModel.CityCode = risk.City.Id;
                if (risk.City.State != null)
                {
                    riskModel.StateCode = risk.City.State.Id;
                    if (risk.City.State != null)
                    {
                        riskModel.CountryCode = risk.City.State.Country.Id;
                    }
                }
            }

            if (risk.NomenclatureAddress != null)
            {
                riskModel.RouteTypeId = risk.NomenclatureAddress.Type.Id;
                riskModel.Name = risk.NomenclatureAddress.Name;
                riskModel.Letter = risk.NomenclatureAddress.Letter;
                riskModel.Letter2 = risk.NomenclatureAddress.Letter2;
                riskModel.Number1 = risk.NomenclatureAddress.Number1;
                riskModel.Number2 = risk.NomenclatureAddress.Number2;
                riskModel.Number3 = risk.NomenclatureAddress.Number3;
                riskModel.ApartmentOrOfficeId = risk.NomenclatureAddress.ApartmentOrOffice.Id;
            }
            return riskModel;
        }

        #endregion

        #region Coverage

        public static CompanyCoverage CreateRiskCoverage(CoverageModelsView coverageModel)
        {
            CompanyCoverage coverage = new CompanyCoverage
            {
                Id = coverageModel.CoverageId,
                Description = coverageModel.Description,
                CurrentFrom = Convert.ToDateTime(coverageModel.CurrentFrom),
                CurrentTo = Convert.ToDateTime(coverageModel.CurrentTo),
                PremiumAmount = coverageModel.PremiumAmount,
                LimitAmount = coverageModel.LimitAmount,
                SubLimitAmount = coverageModel.SubLimitAmount,
                Rate = coverageModel.Rate,
                RateType = (Core.Services.UtilitiesServices.Enums.RateType)coverageModel.RateType,
                CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)coverageModel.CalculationTypeId,
                EndorsementType = (EndorsementType)coverageModel.EndorsementType,
                CoverStatus = (CoverageStatusType)coverageModel.CoverStatus,
                CoverStatusName = coverageModel.CoverStatusName,
                FlatRatePorcentage = coverageModel.FlatRatePorcentage.HasValue ? coverageModel.FlatRatePorcentage.Value : 0,
                IsMandatory = coverageModel.IsMandatory,
                IsSelected = coverageModel.IsSelected,
                IsVisible = coverageModel.IsVisible,
                SubLineBusiness = new CompanySubLineBusiness
                {
                    Id = coverageModel.SubLineBusinessId,
                    LineBusiness = new CompanyLineBusiness
                    {
                        Id = coverageModel.LineBusinessId
                    }
                }
            };

            if (coverageModel.DeductibleId.HasValue)
            {
                coverage.Deductible = new CompanyDeductible
                {
                    Id = coverageModel.DeductibleId.Value,
                    Description = coverageModel.DeductibleDescription
                };
            }

            return coverage;
        }

        #endregion

        #region CompanyLiabilityRisk

        public static CompanyLiabilityRisk CreateLiabilityRisk(RiskLiabilityViewModel riskModel)
        {
            CompanyLiabilityRisk LiabilityRisk = new CompanyLiabilityRisk
            {
                FullAddress = riskModel.FullAddress,
                //   Description = riskModel.FullAddress,
                City = new City
                {
                    Id = riskModel.CityCode,
                    DANECode = riskModel.DaneCode,
                    State = new State
                    {
                        Id = riskModel.StateCode,
                        Country = new Country
                        {
                            Id = riskModel.CountryCode
                        }
                    }
                },
                IsDeclarative = riskModel.IsDeclarative,
                Risk = new CompanyRisk
                {
                    Description = riskModel.FullAddress,
                    RiskActivity = riskModel.RiskActivityId == null ? null : new CompanyRiskActivity { Description = riskModel.RiskActivityDescription, Id = riskModel.RiskActivityId.Value },

                    GroupCoverage = new ISSMODEL.GroupCoverage { Id = riskModel.GroupCoverage },
                    Status = RiskStatusType.Original,
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId,
                        IndividualType = (IndividualType)riskModel.InsuredIndividualType,
                        Name = riskModel.InsuredName,
                        CustomerType = (CustomerType)riskModel.InsuredCustomerType,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            DocumentType = new IssuanceDocumentType { Id = riskModel.InsuredDocumentTypeId },
                            Number = riskModel.InsuredDocumentNumber
                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = riskModel.InsuredDetailId.GetValueOrDefault(),
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = riskModel.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = riskModel.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = riskModel.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        BirthDate = riskModel.InsuredBirthDate,
                        Gender = riskModel.InsuredGender,
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = riskModel.InsuredAssociationType 
                            
                        }
                    },
                    Beneficiaries = new List<CompanyBeneficiary>(),
                    IsPersisted = true,
                    IsRetention = riskModel.IsRetention,
                },

                RiskSubActivity = riskModel.RiskSubActivityId == null ? null : new LBTY.CompanyRiskSubActivity
                {
                    Id = (int)riskModel.RiskSubActivityId,
                    Description = riskModel.RiskSubActivityDescripcion

                },

            };

            if (riskModel.AssuranceModeId != null)
            {
                LiabilityRisk.AssuranceMode = new LBTY.CompanyAssuranceMode
                {
                    Id = (int)riskModel.AssuranceModeId,
                    Description = riskModel.AssuranceModeDescripcion,
                };
            }
            if (riskModel.RiskId.HasValue)
            {
                LiabilityRisk.Risk.Id = riskModel.RiskId.Value;
            }

            if (!string.IsNullOrEmpty(riskModel.AmountInsured))
            {
                LiabilityRisk.Risk.AmountInsured = Convert.ToDecimal(riskModel.AmountInsured);
            }
            if (!string.IsNullOrEmpty(riskModel.Premium))
            {
                LiabilityRisk.Risk.Premium = Convert.ToDecimal(riskModel.Premium);
            }
            if (riskModel.RateZoneDescription != null)
            {
                LiabilityRisk.Risk.RatingZone = new CompanyRatingZone { Id = riskModel.RateZoneId, Description = riskModel.RateZoneDescription };
            }
            return LiabilityRisk;
        }

        public static RiskLiabilityViewModel CreateLiability(CompanyLiabilityRisk risk)
        {
            RiskLiabilityViewModel riskModel = new RiskLiabilityViewModel
            {
                RiskId = risk.Risk.Id,
                GroupCoverage = risk.Risk.GroupCoverage == null ? 0 : risk.Risk.GroupCoverage.Id,
                AmountInsured = risk.Risk.AmountInsured.ToString(),
                Premium = risk.Risk.Premium.ToString(),
                InsuredId = risk.Risk.MainInsured == null ? 0 : risk.Risk.MainInsured.InsuredId,
                InsuredName = risk.Risk.MainInsured == null ? "" : risk.Risk.MainInsured.Name
            };

            return riskModel;
        }




        #endregion

        #region companySurcharges
        public static List<SurchargesViewModel> CreateViewSurcharges(List<UWM.CompanySurchargeComponent> companySurcharges)
        {
            List<SurchargesViewModel> surcharges = new List<SurchargesViewModel>();
            foreach (var item in companySurcharges)
            {
                surcharges.Add(CreateViewSurcharge(item));
            }
            return surcharges;
        }

        public static SurchargesViewModel CreateViewSurcharge(UWM.CompanySurchargeComponent companySurcharge)
        {
            SurchargesViewModel surcharge = new SurchargesViewModel
            {
                Type = (int)companySurcharge.RateType.Id,
                RateDescription = companySurcharge.RateType.Description.ToString(),
                Rate = companySurcharge.Rate.ToString(),
                Id = companySurcharge.Id,
                Description = companySurcharge.Description
            };
            return surcharge;
        }

        public static List<UWM.CompanySurchargeComponent> CreateCompanySurcharges(List<SurchargesViewModel> surchargesView)
        {
            List<UWM.CompanySurchargeComponent> surcharges = new List<UWM.CompanySurchargeComponent>();
            foreach (var item in surchargesView)
            {
                surcharges.Add(CreateCompanySurcharge(item));
            }
            return surcharges;
        }

        public static UWM.CompanySurchargeComponent CreateCompanySurcharge(SurchargesViewModel surchargeView)
        {
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ".";
            clone.NumberFormat.NumberGroupSeparator = ",";

            UWM.CompanySurchargeComponent surcharge = new UWM.CompanySurchargeComponent
            {
                Id = surchargeView.Id,
                Description = surchargeView.Description,
                Rate = (surchargeView.Rate == "") ? Convert.ToDecimal(0) : decimal.Parse(surchargeView.Rate, clone),
                State = (int)(Application.ModelServices.Enums.StatusTypeService)surchargeView.StatusTypeService
            };
            surcharge.RateType = new CompanyRateType
            {
                Id = surchargeView.Type ?? 0,
                Description = surchargeView.RateDescription
            };
            return surcharge;
        }

        public static UWM.CompanySummaryComponent CreateCompanySummarySurcharge(List<SurchargesViewModel> surchargesView)
        {
            if (surchargesView != null)
            {
                var surcharges = CreateCompanySurcharges(surchargesView);
                UWM.CompanySummaryComponent summary = new UWM.CompanySummaryComponent
                {
                    Surcharge = surcharges
                };
                return summary;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region CompanyTplRisk

        public static RiskThirdPartyLiabilityViewModel CreateThirdPartyLiabilityViewModel(CTPLM.CompanyTplRisk risk)
        {
            RiskThirdPartyLiabilityViewModel riskModel = new RiskThirdPartyLiabilityViewModel
            {
                RiskId = risk.Risk.Id,
                GroupCoverage = risk.Risk.GroupCoverage.Id,
                LicensePlate = risk.LicensePlate,
                Engine = risk.EngineSerial,
                Chassis = risk.ChassisSerial,
                Make = risk.Make.Id,
                TypeVehicle = risk.Version.Type.Id,
                RepoweringYear = risk.Year.ToString(),
                Shuttle = risk.Shuttle.Id,
                ServiceType = risk.ServiceType.Id,
                Deductible = risk.Deductible.Id,
                RatingZone = risk.Risk.RatingZone.Id,
                PassengerQuantity = risk.PassengerQuantity,
                RateType = (int)risk.RateType,
                Rate = risk.Rate.ToString(),
                InsuredId = risk.Risk.MainInsured.InsuredId,
                InsuredDocumentNumber = risk.Risk.MainInsured.IdentificationDocument.Number,
                InsuredName = risk.Risk.MainInsured.Name,
                InsuredCustomerType = (int)risk.Risk.MainInsured.CustomerType,
                AmountInsured = risk.Risk.AmountInsured.ToString(),
                Premium = risk.Risk.Premium.ToString(),
                YearModel = risk.YearModel.ToString()
            };

            return riskModel;
        }

        public static CTPLM.CompanyTplRisk CreateThirdPartyLiability(RiskThirdPartyLiabilityViewModel riskViewModel)
        {
            CTPLM.CompanyTplRisk risk = new CTPLM.CompanyTplRisk { Risk = new CompanyRisk() };
            risk.Risk.Description = riskViewModel.LicensePlate == null ? "" : riskViewModel.LicensePlate;
            risk.Risk.RiskId = riskViewModel.OriginalRiskId.GetValueOrDefault();
            risk.Risk.Id = riskViewModel.RiskId.GetValueOrDefault();
            risk.Risk.Status = RiskStatusType.Original;
            risk.Risk.MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
            {
                IndividualId = riskViewModel.InsuredId,
                IndividualType = IndividualType.Person,
                IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                {
                    Number = riskViewModel.InsuredDocumentNumber
                },
                Name = riskViewModel.InsuredName,
                CustomerType = (CustomerType)riskViewModel.InsuredCustomerType,
                CompanyName = new ISSMODEL.IssuanceCompanyName
                {
                    NameNum = riskViewModel.InsuredDetailId.GetValueOrDefault(),
                    Address = new ISSMODEL.IssuanceAddress
                    {
                        Id = riskViewModel.InsuredAddressId
                    },
                    Phone = new ISSMODEL.IssuancePhone
                    {
                        Id = riskViewModel.InsuredPhoneId.GetValueOrDefault()
                    },
                    Email = new ISSMODEL.IssuanceEmail
                    {
                        Id = riskViewModel.InsuredEmailId.GetValueOrDefault()
                    }
                },
                BirthDate = riskViewModel.InsuredBirthDate,
                Gender = riskViewModel.InsuredGender
            };
            risk.Risk.GroupCoverage = new ISSMODEL.GroupCoverage
            {
                Id = riskViewModel.GroupCoverage
            };
            risk.Risk.CoveredRiskType = CoveredRiskType.Vehicle;
            risk.Risk.Description = riskViewModel.LicensePlate;
            risk.LicensePlate = riskViewModel.LicensePlate;
            risk.EngineSerial = riskViewModel.Engine;
            risk.ChassisSerial = riskViewModel.Chassis;
            risk.Make = new VECOM.CompanyMake
            {
                Id = riskViewModel.Make,
                Description = riskViewModel.MakeDescription
            };
            risk.Model = new VECOM.CompanyModel
            {
                Id = riskViewModel.Model
            };
            risk.Version = new VECOM.CompanyVersion
            {
                Id = riskViewModel.Version,
                Type = new VECOM.CompanyType
                {
                    Id = riskViewModel.TypeVehicle
                },
                Body = new VECOM.CompanyBody
                {
                    Id = 2//(NO TIENE)
                }
            };
            risk.Color = new CompanyColor
            {
                Id = 0// riskViewModel.Color (COLOR NO DEFINIDO PARA RC PASAJEROS)
            };
            risk.Year = Convert.ToInt32(riskViewModel.Year);
            risk.Shuttle = new CTPLM.CompanyShuttle
            {
                Id = riskViewModel.Shuttle
            };
            risk.ServiceType = new VECOM.CompanyServiceType
            {
                Id = riskViewModel.ServiceType
            };
            risk.Deductible = new CompanyDeductible
            {
                Id = riskViewModel.Deductible
            };
            risk.Risk.RatingZone = new CompanyRatingZone
            {
                Id = riskViewModel.RatingZone
            };
            risk.Fuel = new CompanyFuel
            {
                Id = 1//(NO APLICA)
            };
            risk.PassengerQuantity = riskViewModel.PassengerQuantity;
            risk.RateType = (Core.Services.UtilitiesServices.Enums.RateType?)riskViewModel.RateType;
            risk.Rate = Convert.ToDecimal(riskViewModel.Rate);
            risk.Risk.AmountInsured = Convert.ToDecimal(riskViewModel.AmountInsured);
            risk.Risk.Premium = Convert.ToDecimal(riskViewModel.Premium);
            risk.Risk.IsPersisted = true;
            risk.TypeCargoId = riskViewModel.TypeCargoId;
            risk.TypeCargodescription = riskViewModel.TypeCargodescription;
            risk.PhoneNumber = riskViewModel.PhoneNumber;
            risk.TrailerQuantity = riskViewModel.TrailerQuantity;
            risk.Tons = riskViewModel.Tons;
            risk.Risk.IsRetention = riskViewModel.IsRetention;
            risk.Risk.IsFacultative = riskViewModel.IsFacultative;
            risk.RePoweredVehicle = riskViewModel.RePoweredVehicle;
            risk.RepoweringYear = Convert.ToInt32(riskViewModel.RepoweringYear);
            risk.GallonTankCapacity = Convert.ToInt32(riskViewModel.GallonTankCapacity);
            risk.YearModel = Convert.ToInt32(riskViewModel.YearModel);
            risk.Fuel = new CompanyFuel
            {
                Id = (int)Helpers.Enums.FuelType.NoAplica,

            };
            risk.Use = new CompanyUse
            {
                Id = (int)Helpers.Enums.UseType.NoAplica,
            };
            return risk;
        }
        public static CompanyBeneficiary CreateBeneficiaryFromInsured(RiskThirdPartyLiabilityViewModel riskModel)
        {
            CreateMapBeneficiaryFromInsured();
            var beneficiary = Mapper.Map<RiskThirdPartyLiabilityViewModel, CompanyBeneficiary>(riskModel);

            beneficiary.BeneficiaryTypeDescription = null; //Helpers.EnumsHelper.GetItemName<BeneficiaryType>(beneficiary.BeneficiaryType.Id);
            return beneficiary;
        }
        #endregion CompanyTplRisk

        #region JudicialSurety

        public static CompanyJudgement CreateCompanyJudgement(RiskJudicialSuretyModelsView riskModel)
        {
            CompanyJudgement companyJudgement = new CompanyJudgement
            {
                Article = new CompanyArticle
                {
                    Id = riskModel.IdArticle
                },
                InsuredActAs = (CapacityOf)riskModel.IdInsuredActsAs,
                HolderActAs = (CapacityOf)riskModel.IdHolderActAs,


                Court = new CompanyCourt
                {
                    Id = riskModel.IdTypeOfCourt
                },
                City = new City
                {
                    Id = riskModel.IdDepartment,
                    State = new State
                    {
                        Id = riskModel.IdMunicipality,
                        Country = new Country
                        {
                            Id = riskModel.CountryId
                        }
                    },
                },

                SettledNumber = riskModel.ProcessAndOrFiled,
                InsuredValue = Convert.ToDecimal(riskModel.InsuredValue),


                Risk = new CompanyRisk
                {
                    Id = riskModel.RiskId.GetValueOrDefault(),
                    CoveredRiskType = CoveredRiskType.Surety,
                    Status = RiskStatusType.Original,
                    AmountInsured = decimal.Parse(riskModel.AmountInsured),
                    RiskId = riskModel.OriginalRiskId.GetValueOrDefault(),
                    IsPersisted = true,
                    Premium = Convert.ToDecimal(riskModel.Premium),
                    GroupCoverage = new ISSMODEL.GroupCoverage
                    {
                        Id = riskModel.GroupCoverage
                    },
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId.Value,
                        IndividualType = (IndividualType)riskModel.InsuredIndividualTypeId,
                        Name = riskModel.InsuredName,
                        CustomerType = (CustomerType)riskModel.InsuredCustomerType,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            DocumentType = new IssuanceDocumentType { Id = riskModel.InsuredDocumentTypeId },
                            Number = riskModel.InsuredDocumentNumber
                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = riskModel.InsuredDetailId.GetValueOrDefault(),
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = riskModel.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = riskModel.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = riskModel.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        BirthDate = riskModel.InsuredBirthDate,
                        Gender = riskModel.InsuredGender,
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = riskModel.InsuredAssociationType

                        }
                    },
                    SecondInsured = new CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId.Value,
                        Name = riskModel.InsuredName,
                        IndividualType = IndividualType.Person,
                        CustomerType = CustomerType.Individual,
                    },
                    IsRetention = riskModel.IsRetention,
                    IsFacultative = riskModel.IsRetention,
                    RiskActivity = new CompanyRiskActivity
                    {
                        Id = riskModel.RiskActivityId
                    }
                },

            };
            if (riskModel.Attorney != null)
            {
                companyJudgement.Attorney = new CompanyAttorney
                {
                    IdProfessionalCard = riskModel.Attorney.CardProfessional,
                    Name = riskModel.Attorney.AttorneyName,
                    IdentificationDocument = new IdentificationDocument { Number = riskModel.Attorney.NumberId, DocumentType = new DocumentType { Id = riskModel.Attorney.DocumentType } }
                };
            }
            return companyJudgement;
        }

        public static CompanyCoverage CreateCoverageModelsViewToCompanyCoverage(CompanyCoverage coverage, CoverageModelsView coverageModel)

        {
            coverage.Id = coverageModel.CoverageId;
            coverage.CurrentFrom = Convert.ToDateTime(coverageModel.CurrentFrom);
            coverage.CurrentTo = Convert.ToDateTime(coverageModel.CurrentTo);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)coverageModel.CalculationTypeId;
            coverage.LimitAmount = coverageModel.LimitAmount;
            coverage.SubLimitAmount = coverageModel.SubLimitAmount;
            coverage.Rate = coverageModel.Rate;
            coverage.RateType = (Core.Services.UtilitiesServices.Enums.RateType)coverageModel.RateType;
            coverage.PremiumAmount = coverageModel.PremiumAmount;
            coverage.DeclaredAmount = coverageModel.DeclaredAmount;
            coverage.MaxLiabilityAmount = Convert.ToDecimal(coverageModel.MaxLiabilityAmount);
            if (coverage.InsuredObject != null)
            {
                coverage.InsuredObject.Amount = coverageModel.SubLimitAmount;
            }

            coverage.LimitOccurrenceAmount = Convert.ToDecimal(coverageModel.LimitOccurrenceAmount);
            coverage.LimitClaimantAmount = Convert.ToDecimal(coverageModel.LimitClaimantAmount);
            if (coverageModel.DeductibleId.GetValueOrDefault() > 0)
            {
                coverage.Deductible = new CompanyDeductible
                {
                    Id = coverageModel.DeductibleId.Value,
                    Description = coverageModel.DeductibleDescription
                };
            }

            return coverage;
        }

        #endregion JudicialSurety
        #region Ramo Cumplimiento
        #region policySurety
        public static PolicyModelsView CreateSuretyPolicy(CompanyPolicy companyPolicy)
        {
            CreateMapSuretyPolicy();
            return Mapper.Map<CompanyPolicy, PolicyModelsView>(companyPolicy);
        }
        #endregion
        #region RiskSurety
        public static CSUM.CompanyContract CreateRiskSurety(RiskSuretyModelsView riskSuretyModel)
        {
            var mapper = CreateMapSurety();
            var companyContract = mapper.Map<RiskSuretyModelsView, CSUM.CompanyContract>(riskSuretyModel);
            //    companyContract.ContractObject = new CompanyText
            //    {
            //        TextBody = riskSuretyModel.ContractObject
            //}; 
            //companyContract.Risk.MainInsured = new CompanyIssuanceInsured { IndividualType = IndividualType.Person, CustomerType = CustomerType.Individual };
            //companyContract.Risk.MainInsured.IndividualType = IndividualType.Person;
            //companyContract.Risk.MainInsured.CustomerType = CustomerType.Individual;
            //companyContract.RiskSuretyPost.UserId = SessionHelper.GetUserId();

            return companyContract;
        }
        #region RiskSuretyContract
        /// <summary>
        /// Creates the risk surety.
        /// </summary>
        /// <param name="riskSurety">The risk surety.</param>
        /// <returns></returns>
        public static RiskSuretyModelsView CreateRiskSurety(CompanyContract riskSurety)
        {
            var immaper = CreateMapSuretyModelView();
            return immaper.Map<CSUM.CompanyContract, RiskSuretyModelsView>(riskSurety);
        }

        #endregion
        #endregion  RiskSurety
        #endregion  Ramo Cumplimiento
        #region Autommmaper
        #region Ramo Cumplimiento
        public static IMapper CreateMapSurety()
        {
            var config = MapperCache.GetMapper<RiskSuretyModelsView, CSUM.CompanyContract>(cfg =>
            {
                cfg.CreateMap<RiskSuretyModelsView, CSUM.CompanyContract>()
                .ForMember(des => des.ContractObject, ori => ori.MapFrom(src => new CompanyText
                {
                    TextBody = src.ContractObject
                }))
                .ForMember(des => des.Risk, ori => ori.MapFrom(scr => new CompanyRisk
                {
                    Id = scr.RiskId.GetValueOrDefault(),
                    Premium = Convert.ToDecimal(String.IsNullOrEmpty(scr.Premium) ? "" : scr.Premium),
                    AmountInsured = Convert.ToDecimal(String.IsNullOrEmpty(scr.Premium) ? "" : scr.AmountInsured),
                    Description = scr.ContractorName,
                    GroupCoverage = new ISSMODEL.GroupCoverage
                    {
                        Id = scr.GroupCoverage
                    },
                    Status = RiskStatusType.Original,
                    CoveredRiskType = CoveredRiskType.Surety,
                    IsPersisted = true,
                    Policy = new CompanyPolicy { IsPersisted = false, Endorsement = new CompanyEndorsement { TemporalId = scr.TemporalId } },
                    IsNational = scr.IsNational,
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = scr.InsuredId,
                        IndividualType =(IndividualType)scr.InsuredIndividualType,
                        Name = scr.InsuredName,
                        CustomerType = (CustomerType)scr.InsuredCustomerType,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            Number = scr.InsuredIdentificationDocument,
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = scr.InsuredDocumentTypeId,
                                Description = scr.InsuredDocumentTypeDescription
                            },

                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = scr.InsuredDetailId ?? 0,
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = scr.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = scr.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = scr.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        AssociationType = new IssuanceAssociationType()
                        {
                            Id = scr.InsuredAssociationTypeId != null ? Convert.ToInt32(scr.InsuredAssociationTypeId) : default(int)
                        },
                        BirthDate = scr.InsuredBirthDate,
                        Gender = scr.InsuredGender
                    }
                }))
                .ForMember(des => des.Contractor, ori => ori.MapFrom(scr => new CompanyContractor
                {
                     IndividualId = scr.ContractorId,
                     IndividualType = (IndividualType)scr.ContractorIndividualType,
                     CustomerType = (CustomerType)scr.ContractorCustomerType,
                     Name = scr.ContractorName,
                     InsuredId = scr.ContractorInsuredId,
                     IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                     {
                         Number = scr.ContractorIdentificationDocument,
                         DocumentType = new IssuanceDocumentType
                         {
                             Description = scr.ContractorDocumentTypeDescription,
                             Id = scr.ContractorDocumentTypeId
                         }
                     },
                     CompanyName = new ISSMODEL.IssuanceCompanyName
                     {
                         NameNum = scr.ContractorDetailId.GetValueOrDefault(),
                         Address = new ISSMODEL.IssuanceAddress
                         {
                             Description = scr.ContractorAddressDescription,
                             Id = scr.ContractorAddressId
                         },
                         Phone = new ISSMODEL.IssuancePhone
                         {
                             Description = scr.ContractorPhoneDescription
                         }
                     },
                     AssociationType = new IssuanceAssociationType
                     {
                         Id = scr.ContractorAssociationTypeId !=null ? (int)scr.ContractorAssociationTypeId : default(int)
                     }
                }))
                .ForMember(des => des.Class, ori => ori.MapFrom(scr => new CompanyContractClass
                {
                    Id = scr.Class
                }))
                .ForMember(des => des.ContractType, ori => ori.MapFrom(scr => new CompanyContractType
                {
                    Id = scr.ContractType
                }))
                .ForMember(des => des.Value, ori => ori.MapFrom(scr => new Amount
                {
                    Value = scr.ContractValue
                }))
                .ForMember(des => des.OperatingQuota, ori => ori.MapFrom(scr => new OperatingQuota
                {
                    Amount = scr.OperatingQuota
                }))
                .ForMember(des => des.Country, ori => ori.MapFrom(scr => new Country
                {
                  Id = scr.CountryId,
                  Description = scr.CountryDescription
                }))
               .ForMember(des => des.State, ori => ori.MapFrom(scr => new State
               {
                   Id = scr.StateId == null ? 0 : (int)scr.StateId,
                   Description = scr.StateDescription
               }))
                 .ForMember(des => des.City, ori => ori.MapFrom(scr => new City
                 {
                     Id = scr.CityId == null ? 0 : (int)scr.CityId,
                     Description = scr.CityDescription
                 }))
                .ForMember(des => des.Aggregate, ori => ori.MapFrom(scr => scr.Aggregate))
                .ForMember(des => des.Available, ori => ori.MapFrom(scr => scr.Available))
                .ForMember(des => des.Isfacultative, ori => ori.MapFrom(scr => scr.IsfacultativeId))
                .ForMember(des => des.IsRetention, ori => ori.MapFrom(scr => scr.IsRetention))
                .ForMember(des => des.RiskSuretyPost, ori => ori.MapFrom(src => new CompanyRiskSuretyPost
                {
                    TempId = src.TemporalId,
                    ContractDate = src.TerminalUnitContract,
                    IssueDate = src.FinalDeliveryDate,
                    ChkContractDate = src.ChkContractDate,
                    ChkContractFinalyDate = src.ChkContractFinalyDate,
                    UserId = SessionHelper.GetUserId(true)
                }))

                .ForMember(des => des.SettledNumber, ori => ori.MapFrom(scr => String.IsNullOrEmpty(scr.ContractNumber) ? "" : scr.ContractNumber));
                });
                return config;
        }
        public static IMapper CreateMapSuretyModelView()
        {
            return MapperCache.GetMapper<CSUM.CompanyContract, RiskSuretyModelsView>(cfg =>
            {
                cfg.CreateMap<CSUM.CompanyContract, RiskSuretyModelsView>()
            .ForMember(des => des.TemporalId, ori => ori.MapFrom(scr => scr.Risk.Id))
            .ForMember(des => des.RiskId, ori => ori.MapFrom(scr => scr.Risk))
            .ForMember(des => des.IsfacultativeId, ori => ori.MapFrom(scr => scr.Isfacultative))
            .ForMember(des => des.ContractNumber, ori => ori.MapFrom(scr => String.IsNullOrEmpty(scr.SettledNumber) ? "" : scr.SettledNumber))
            .ForMember(des => des.ContractorName, ori => ori.MapFrom(scr => scr.Risk.Description))
            .ForMember(des => des.Class, ori => ori.MapFrom(scr => scr.Class.Id))
            .ForMember(des => des.ContractType, ori => ori.MapFrom(scr => scr.ContractType.Id))
            .ForMember(des => des.ContractValue, ori => ori.MapFrom(scr => scr.Value.Value))
            .ForMember(des => des.OperatingQuota, ori => ori.MapFrom(scr => scr.OperatingQuota.Amount))
            .ForMember(des => des.Aggregate, ori => ori.MapFrom(scr => scr.Aggregate))
            .ForMember(des => des.Available, ori => ori.MapFrom(scr => scr.Available))
            .ForMember(des => des.GroupCoverage, ori => ori.MapFrom(scr => scr.Risk.GroupCoverage.Id))
            .ForMember(des => des.Premium, ori => ori.MapFrom(scr => scr.Risk.Premium))
            .ForMember(des => des.InsuredId, ori => ori.MapFrom(scr => scr.Risk.MainInsured.IndividualId))
            .ForMember(des => des.InsuredName, ori => ori.MapFrom(scr => scr.Risk.MainInsured.Name))
            .ForMember(des => des.InsuredCustomerType, ori => ori.MapFrom(scr => (int)scr.Risk.MainInsured.CustomerType))
            .ForMember(des => des.InsuredIdentificationDocument, ori => ori.MapFrom(scr => scr.Risk.MainInsured.IdentificationDocument.Number))
            .ForMember(des => des.InsuredDetailId, ori => ori.MapFrom(scr => scr.Risk.MainInsured.CompanyName.NameNum))

            .ForMember(des => des.ContractorId, ori => ori.MapFrom(scr => scr.Contractor.IndividualId))
            .ForMember(des => des.ContractorName, ori => ori.MapFrom(scr => scr.Contractor.Name))
            .ForMember(des => des.ContractorIdentificationDocument, ori => ori.MapFrom(scr => scr.Contractor.IdentificationDocument.Number))
            .ForMember(des => des.ContractorDetailId, ori => ori.MapFrom(scr => scr.Contractor.CompanyName.NameNum));
            });

        }
        #endregion
        #region policy
        public static IMapper CreateMapSuretyPolicy()
        {
            var config = MapperCache.GetMapper<CompanyPolicy, PolicyModelsView>(cfg =>
            {
                cfg.CreateMap<CompanyPolicy, PolicyModelsView>()
            .ForMember(dest => dest.ProductId, ori => ori.MapFrom(scr => scr.Product.Id))
            .ForMember(dest => dest.PrefixId, ori => ori.MapFrom(scr => scr.Prefix.Id))
            .ForMember(dest => dest.PolicyType, ori => ori.MapFrom(scr => scr.PolicyType.Id))
            .ForMember(dest => dest.Id, ori => ori.MapFrom(scr => scr.Id))
            .ForMember(dest => dest.EndorsementType, ori => ori.MapFrom(scr => (int?)scr.Endorsement.EndorsementType))
            .ForMember(dest => dest.HolderId, ori => ori.MapFrom(scr => scr.Holder.IndividualId))
            .ForMember(dest => dest.CurrentFrom, ori => ori.MapFrom(scr => scr.CurrentFrom))
            .ForMember(dest => dest.CurrentTo, ori => ori.MapFrom(scr => scr.CurrentTo))
            .ForMember(dest => dest.IssueDate, ori => ori.MapFrom(scr => scr.IssueDate));
            });
            return config;
        }

        public static IMapper CreateMapSuretyPolicyView()
        {
            var config = MapperCache.GetMapper<PolicyModelsView, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<PolicyModelsView, CompanyPolicy>()
            .ForMember(dest => dest.Product, ori => ori.MapFrom(scr => new CompanyProduct { Id = scr.ProductId }))
            .ForMember(dest => dest.Prefix, ori => ori.MapFrom(scr => new CompanyPrefix { Id = scr.PrefixId }))
            .ForMember(dest => dest.PolicyType, ori => ori.MapFrom(scr => new PolicyType { Id = scr.PolicyType }))
            .ForMember(dest => dest.Id, ori => ori.MapFrom(scr => scr.Id))
            .ForMember(dest => dest.Endorsement, ori => ori.MapFrom(scr => new CompanyEndorsement { EndorsementType = (EndorsementType?)scr.EndorsementType }))
            .ForMember(dest => dest.Holder, ori => ori.MapFrom(scr => new ISSMODEL.Holder { IndividualId = scr.HolderId }))
            .ForMember(dest => dest.CurrentFrom, ori => ori.MapFrom(scr => scr.CurrentFrom))
            .ForMember(dest => dest.CurrentTo, ori => ori.MapFrom(scr => scr.CurrentTo))
            .ForMember(dest => dest.IssueDate, ori => ori.MapFrom(scr => scr.IssueDate));
            });
            return config;
        }
        #endregion
        #region CompanyMapTplRisk
        public static IMapper CreateMapBeneficiaryFromInsured()
        {
            var config = MapperCache.GetMapper<RiskThirdPartyLiabilityViewModel, CompanyBeneficiary>(cfg =>
            {

                cfg.CreateMap<RiskThirdPartyLiabilityViewModel, CompanyBeneficiary>()
                .ForMember(dest => dest.IndividualId, ori => ori.MapFrom(scr => scr.InsuredId))
                .ForMember(dest => dest.IdentificationDocument, ori => ori.MapFrom(scr => new IdentificationDocument
                {
                    Number = scr.InsuredDocumentNumber
                }))
                   .ForMember(dest => dest.Name, ori => ori.MapFrom(scr => scr.InsuredName))
                   .ForMember(dest => dest.Participation, ori => ori.MapFrom(scr => 100))

                   .ForMember(dest => dest.CustomerType, ori => ori.MapFrom(scr => (CustomerType)scr.InsuredCustomerType)).ForMember(dest => dest.CompanyName, ori => ori.MapFrom(scr => new CompanyName
                   {
                       NameNum = scr.InsuredDetailId.GetValueOrDefault(),
                       Address = new Address
                       {
                           Id = scr.InsuredAddressId
                       },
                       Phone = new Phone
                       {
                           Id = scr.InsuredPhoneId.GetValueOrDefault()
                       },
                       Email = new Email
                       {
                           Id = scr.InsuredEmailId.GetValueOrDefault()
                       }
                   }));
            });
            return config;

        }


        #endregion
        #region property
        public static IMapper CreateMapProperty()
        {
            var config = MapperCache.GetMapper<RiskPropertyViewModel, CompanyInsuredObject>(cfg =>
            {
                cfg.CreateMap<RiskPropertyViewModel, CompanyInsuredObject>();
            });
            return config;
        }
        public static IMapper CreateMapAdditionalData()
        {
            var config = MapperCache.GetMapper<PropertyAdditionalDataViewModel, PROP.CompanyPropertyRisk>(cfg =>
            {
                cfg.CreateMap<PropertyAdditionalDataViewModel, PROP.CompanyPropertyRisk>()
            .ForMember(dest => dest.ConstructionType, scr => scr.MapFrom(ori => new PROP.CompanyConstructionType { Id = ori.ConstructionType }))
            .ForMember(dest => dest.RiskType, scr => scr.MapFrom(ori => new ISSMODEL.RiskType { Id = ori.RiskType }))
            .ForMember(dest => dest.RiskUse, scr => scr.MapFrom(ori => new CLOCATIONMODEL.CompanyRiskUse { Id = ori.RiskUse }));

            });
            return config;
        }
        #endregion
        #region Caucion Judicial
        public static IMapper CreateMapAdditionalDataJudicialSurety()
        {
            var config = MapperCache.GetMapper<AdditionalDataJudicialSuretyModelsView, CompanyJudgement>(cfg =>
            {
                cfg.CreateMap<AdditionalDataJudicialSuretyModelsView, CompanyJudgement>()
            .ForMember(dest => dest.Risk, scr => scr.MapFrom(ori => new CompanyRisk
            {
                Id = ori.RiskId,
                SecondInsured = new CompanyIssuanceInsured
                {
                    IndividualId = ori.AttorneyId.HasValue == false ? 0 : (int)ori.AttorneyId.Value,
                    Name = ori.AttorneyName,
                    IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                    {
                        Number = ori.NumberId.ToString(),
                        DocumentType = new ISSMODEL.IssuanceDocumentType { Id = ori.DocumentType }
                    },
                    IndividualType = IndividualType.Person,
                    CustomerType = CustomerType.Individual

                }

            }))
            .ForMember(dest => dest.InsuredActAs, scr => scr.MapFrom(ori => CapacityOf.Applicant))
            .ForMember(dest => dest.HolderActAs, scr => scr.MapFrom(ori => CapacityOf.Applicant))
            .ForMember(dest => dest.Attorney, scr => scr.MapFrom(ori => new CompanyAttorney
            {
                Id = ori.AttorneyId.HasValue == false ? 0 : (int)ori.AttorneyId.Value,
                Name = ori.AttorneyName,
                IdentificationDocument = new IdentificationDocument { Number = ori.NumberId.ToString(), DocumentType = new DocumentType { Id = ori.DocumentType } },
                IdProfessionalCard = ori.CardProfessional,
                InsuredToPrint = ori.InsuredToPrint
            }));
            });
            return config;
        }
        #endregion Caucion Judicial
        #endregion Autommmaper


        #region Company Tables Expreses
        public static List<TablesExpresesModelsView> MappTablesExpreses(ExpensesServiceModel expensesServiceModel)
        {
            List<TablesExpresesModelsView> listTablesExpresesModelsView = new List<TablesExpresesModelsView>();
            foreach (ExpenseServiceModel item in expensesServiceModel.ComponentServiceModel)
            {
                TablesExpresesModelsView tablesExpresesModelsView = new TablesExpresesModelsView();
                tablesExpresesModelsView.Id = item.Id;
                tablesExpresesModelsView.Description = item.SmallDescription;
                tablesExpresesModelsView.IsInitiallyIncluded = item.IsInitially;
                tablesExpresesModelsView.IsObligatory = item.IsMandatory;
                tablesExpresesModelsView.Taxes = item.Rate;
                RateTypeServiceQueryModel rateTypeServiceQueryModel = new RateTypeServiceQueryModel
                {
                    Id = item.RateTypeServiceQueryModel.Id,
                    description = item.RateTypeServiceQueryModel.description
                };
                tablesExpresesModelsView.TaxesTypeId = rateTypeServiceQueryModel.Id;
                tablesExpresesModelsView.TaxesType = rateTypeServiceQueryModel.description;
                RuleSetServiceQueryModel ruleSetServiceQueryModel = new RuleSetServiceQueryModel
                {
                    Id = item.RuleSetServiceQueryModel.Id,
                    description = item.RuleSetServiceQueryModel.description

                };
                tablesExpresesModelsView.RuleId = ruleSetServiceQueryModel.Id;
                tablesExpresesModelsView.RuleDescription = ruleSetServiceQueryModel.description;
                listTablesExpresesModelsView.Add(tablesExpresesModelsView);
            }
            return listTablesExpresesModelsView;
        }
        #endregion
        #region Company Discount
        public static List<DiscountsModelsView> MappCreateDiscount(DiscountsServiceModel discountsModelView)
        {
            List<DiscountsModelsView> listDiscountsModelsViews = new List<DiscountsModelsView>();
            foreach (DiscountServiceModel item in discountsModelView.DiscountServiceModel)
            {
                DiscountsModelsView discountsModelsView = new DiscountsModelsView();
                discountsModelsView.Id = item.Id;
                discountsModelsView.Description = item.Description;
                discountsModelsView.TaxeType = Convert.ToInt16(item.Type);
                discountsModelsView.Taxe = Convert.ToString(item.Rate);
                listDiscountsModelsViews.Add(discountsModelsView);
            }
            return listDiscountsModelsViews;

        }

        public static CompanySummaryComponent MappCreateCompanySummaryDiscount(List<DiscountsModelsView> discountsModelsViews)
        {
            List<CompanyDiscountComponent> listDiscountsModelsView = new List<CompanyDiscountComponent>();
            CompanySummaryComponent companySummaryComponent = new CompanySummaryComponent();
            foreach (DiscountsModelsView item in discountsModelsViews)
            {
                CompanyDiscountComponent companyDiscountComponent = new CompanyDiscountComponent();
                companyDiscountComponent.Id = item.Id;
                companyDiscountComponent.Description = item.Description;
                CompanyRateType companyRateType = new CompanyRateType
                {
                    Id = item.TaxeType,
                    Description = item.TaxeTypeDescription
                };
                companyDiscountComponent.RateType = companyRateType;
                companyDiscountComponent.Rate = Convert.ToDecimal(item.Taxe);
                listDiscountsModelsView.Add(companyDiscountComponent);
            }
            companySummaryComponent.Discount = listDiscountsModelsView;
            return companySummaryComponent;
        }
        #endregion


        #region SearchQuotation
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de ramo: tabla comm.prefix
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreatePrefixes(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "PrefixCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value

            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de sucursal: tabla comm.branch
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateBranchs(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "BranchCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value

            }).ToList();
        }
        #endregion

        #region CrudServices
        public static List<Dictionary<string, string>> DynamicToDictionaryList(IEnumerable<dynamic> Items)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (var item in Items)
            {
                result.Add(DynamicToDictionary(item));
            }
            return result;
        }
        public static List<Dictionary<string, string>> DynamicToDictionaryList(IEnumerable<dynamic> Items, Dictionary<string, string> filters)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (var item in Items)
            {
                Dictionary<string, string> subItem = DynamicToDictionary(item);
                if (subItem.Keys.Contains(filters.Keys.ElementAt(0)) && subItem[filters.Keys.ElementAt(0)].Equals(filters.Values.ElementAt(0)))
                {
                    result.Add(subItem);
                }
            }
            return result;
        }
        public static Dictionary<string, string> DynamicToDictionary(dynamic item)
        {
            return ((IEnumerable<KeyValuePair<string, JToken>>)item).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()); ;
        }
        #endregion

        #region subscriptionSearch

        public static List<QuotationSearchModelsView> CreateQuotationSearchModelsView(List<CompanyQuotationSearch> companyQuotationSearches)
        {
            List<QuotationSearchModelsView> listQuotationsSearchModelsView = new List<QuotationSearchModelsView>();
            foreach (CompanyQuotationSearch companyQuotationSearch in companyQuotationSearches)
            {
                listQuotationsSearchModelsView.Add(CreateQuotationSearch(companyQuotationSearch));
            }
            return listQuotationsSearchModelsView;
        }

        internal static QuotationSearchModelsView CreateQuotationSearch(CompanyQuotationSearch companyQuotationSearch)
        {
            return new QuotationSearchModelsView()
            {
                QuotationNumber = companyQuotationSearch.QuotationNumber,
                Version = companyQuotationSearch.Version,
                PrefixCommercial = companyQuotationSearch.PrefixCommercial,
                Insured = companyQuotationSearch.Insured,
                Branch = companyQuotationSearch.Branch,
                CurrencyIssuance = companyQuotationSearch.CurrencyIssuance,
                TotalPremium = companyQuotationSearch.TotalPremium,
                User = companyQuotationSearch.User,
                Date = companyQuotationSearch.Date,
                Days = companyQuotationSearch.Days,
                AgentPrincipal = companyQuotationSearch.AgentPrincipal,
                Product = companyQuotationSearch.Product,
                OperationId = companyQuotationSearch.OperationId
            };
        }

        public static List<PolicySearchModelsView> CreatePolicySearchModelsView(List<CompanyPolicySearch> listCompanyPolicySearch)
        {
            List<PolicySearchModelsView> listPolicySearchModelsView = new List<PolicySearchModelsView>();
            foreach (CompanyPolicySearch companyPolicySearch in listCompanyPolicySearch)
            {
                listPolicySearchModelsView.Add(CreatePolicySearch(companyPolicySearch));
            }
            return listPolicySearchModelsView;
        }

        internal static PolicySearchModelsView CreatePolicySearch(CompanyPolicySearch companyPolicySearch)
        {
            return new PolicySearchModelsView()
            {
                PolicyNumber = companyPolicySearch.PolicyNumber,
                Endorsement = companyPolicySearch.EndorsementId,
                PrefixCommercial = companyPolicySearch.PrefixCommercial,
                EndorsementType = companyPolicySearch.EndorsementType,
                Insured = companyPolicySearch.Insured,
                Branch = companyPolicySearch.Branch,
                IssueCurrency = companyPolicySearch.IssueCurrency,
                TotalPremium = companyPolicySearch.TotalPremium,
                User = companyPolicySearch.User,
                IssueDate = companyPolicySearch.IssueDate,
                AgentPrincipal = companyPolicySearch.AgentPrincipal,
                Product = companyPolicySearch.Product
            };
        }

        public static List<TemporalSearchModelsView> CreateTemporalSearchModelsView(List<CompanyTemporalSearch> listCompanyTemporalSearch)
        {
            List<TemporalSearchModelsView> listTemporalSearchModelsView = new List<TemporalSearchModelsView>();
            foreach (CompanyTemporalSearch companyTemporalSearch in listCompanyTemporalSearch)
            {
                listTemporalSearchModelsView.Add(CreateTemporalSearch(companyTemporalSearch));
            }
            return listTemporalSearchModelsView;
        }

        internal static TemporalSearchModelsView CreateTemporalSearch(CompanyTemporalSearch companyTemporalSearch)
        {
            return new TemporalSearchModelsView()
            {
                NumberTemporary = companyTemporalSearch.NumberTemporary,
                PolicyNumber = companyTemporalSearch.PolicyNumber,
                PrefixCommercial = companyTemporalSearch.PrefixCommercial,
                Insured = companyTemporalSearch.Insured,
                Branch = companyTemporalSearch.Branch,
                User = companyTemporalSearch.User,
                ConsultationDate = companyTemporalSearch.ConsultationDate,
                Days = companyTemporalSearch.Days,
                AgentPrincipal = companyTemporalSearch.AgentPrincipal,
                TypeTransaction = companyTemporalSearch.TypeTransaction
            };
        }

        public static CompanySubscriptionSearch CreateCompanySubscriptionSearch(SubscriptionSearchViewModel subscriptionSearchViewModel)
        {
            return new CompanySubscriptionSearch
            {
                HolderId = subscriptionSearchViewModel.HolderId,
                InsuredId = subscriptionSearchViewModel.InsuredId,
                AgentPrincipalId = subscriptionSearchViewModel.AgentPrincipalId,
                AgentAgency = subscriptionSearchViewModel.AgentAgency,
                QuotationNumber = subscriptionSearchViewModel.QuotationNumber,
                Version = subscriptionSearchViewModel.Version,
                Plate = subscriptionSearchViewModel.Plate,
                Engine = subscriptionSearchViewModel.Engine,
                Chassis = subscriptionSearchViewModel.Chassis,
                UserId = subscriptionSearchViewModel.UserId,
                BranchId = subscriptionSearchViewModel.BranchId,
                PrefixId = subscriptionSearchViewModel.PrefixId,
                PolicyNumber = subscriptionSearchViewModel.PolicyNumber,
                EndorsementId = subscriptionSearchViewModel.EndorsementId,
                TemporaryNumber = subscriptionSearchViewModel.TemporaryNumber,
                IssueDate = subscriptionSearchViewModel.IssueDate
            };
        }

        #endregion

        #region Reportes
        public static CompanyProductionReport MappCreateProductionReports(ProductionReportViewModel ProductionReportViewModel)
        {
            return new CompanyProductionReport
            {
                BranchId = ProductionReportViewModel.BranchId,
                PrefixId = ProductionReportViewModel.PrefixId,
                AgentId = ProductionReportViewModel.AgentId,
                ProductId = ProductionReportViewModel.ProductId,
                InputFromDateTime = ProductionReportViewModel.InputFromDateTime,
                InputToDateTime = ProductionReportViewModel.InputToDateTime,
                UserId = ProductionReportViewModel.UserId
            };
        }
        #endregion

        #region Fidelity
        public static CompanyFidelityRisk CreateFidelityRisk(RiskFidelityViewModel riskModel)
        {
            CompanyFidelityRisk FidelityRisk = new CompanyFidelityRisk
            {
                IdOccupation = riskModel.IdOccupation,
                IsDeclarative = riskModel.IsDeclarative,
                DiscoveryDate = riskModel.DiscoveryDate,
                Description = riskModel.OccupationDescription,
                Risk = new CompanyRisk
                {
                    Description = riskModel.OccupationDescription,
                    RiskActivity = new CompanyRiskActivity { Description = riskModel.RiskActivityDescription, Id = riskModel.RiskActivityId == null ? 0 : riskModel.RiskActivityId.Value },
                    GroupCoverage = new ISSMODEL.GroupCoverage { Id = riskModel.GroupCoverage },
                    Status = RiskStatusType.Original,
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = riskModel.InsuredId,
                        IndividualType = (IndividualType)riskModel.InsuredDocumentTypeId,
                        Name = riskModel.InsuredName,
                        CustomerType = (CustomerType)riskModel.InsuredCustomerType,
                        IdentificationDocument = new ISSMODEL.IssuanceIdentificationDocument
                        {
                            Number = riskModel.InsuredDocumentNumber
                        },
                        CompanyName = new ISSMODEL.IssuanceCompanyName
                        {
                            NameNum = riskModel.InsuredDetailId.GetValueOrDefault(),
                            Address = new ISSMODEL.IssuanceAddress
                            {
                                Id = riskModel.InsuredAddressId
                            },
                            Phone = new ISSMODEL.IssuancePhone
                            {
                                Id = riskModel.InsuredPhoneId.GetValueOrDefault()
                            },
                            Email = new ISSMODEL.IssuanceEmail
                            {
                                Id = riskModel.InsuredEmailId.GetValueOrDefault()
                            }
                        },
                        BirthDate = riskModel.InsuredBirthDate ?? DateTime.MinValue,
                        Gender = riskModel.InsuredGender
                    },
                    Beneficiaries = new List<CompanyBeneficiary>(),
                    IsPersisted = true
                },

            };

            if (riskModel.RiskId.HasValue)
            {
                FidelityRisk.Risk.Id = riskModel.RiskId.Value;
            }

            if (!string.IsNullOrEmpty(riskModel.AmountInsured))
            {
                FidelityRisk.Risk.AmountInsured = Convert.ToDecimal(riskModel.AmountInsured);
            }
            if (!string.IsNullOrEmpty(riskModel.Premium))
            {
                FidelityRisk.Risk.Premium = Convert.ToDecimal(riskModel.Premium);
            }

            return FidelityRisk;
        }
        #endregion

    }
}