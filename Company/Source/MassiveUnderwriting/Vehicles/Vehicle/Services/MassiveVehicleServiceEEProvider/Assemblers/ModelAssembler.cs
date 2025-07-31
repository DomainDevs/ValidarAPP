//using Sistran.Company.Application.SyBaseEntityService.Models;
using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {


        public static CompanyAccessory CreateAccesorie(Accessory companyAccessory)
        {
            return new CompanyAccessory
            {
                AccessoryId = companyAccessory.AccessoryId,
                AccumulatedPremium = companyAccessory.AccumulatedPremium,
                Amount = companyAccessory.Amount,
                Description = companyAccessory.Description,
                Enable = companyAccessory.Enable,
                ExtendedProperties = companyAccessory.ExtendedProperties,
                Id = companyAccessory.Id,
                IsOriginal = companyAccessory.IsOriginal,
                Make = companyAccessory.Make,
                OriginalAmount = companyAccessory.OriginalAmount,
                Premium = companyAccessory.Premium,
                Rate = companyAccessory.Rate,
                RateType = Core.Services.UtilitiesServices.Enums.RateType.FixedValue
            };
        }

        public static List<CompanyAccessory> CreateAccesories(List<Accessory> companyAccessory)
        {
            List<CompanyAccessory> accessories = new List<CompanyAccessory>();
            foreach (Accessory companyAccessorys in companyAccessory)
            {
                accessories.Add(CreateAccesorie(companyAccessorys));
            }
            return accessories;
        }

        public static Beneficiary CreateBeneficiary(CompanyBeneficiary companyBeneficiary)
        {
            return new Beneficiary
            {
                BeneficiaryType = new BeneficiaryType { Id = companyBeneficiary.BeneficiaryType.Id },
                BeneficiaryTypeDescription = companyBeneficiary.BeneficiaryTypeDescription,
                CompanyName = companyBeneficiary.CompanyName,
                CustomerType = companyBeneficiary.CustomerType,
                ExtendedProperties = companyBeneficiary.ExtendedProperties,
                IdentificationDocument = companyBeneficiary.IdentificationDocument,
                IndividualId = companyBeneficiary.IndividualId,
                Name = companyBeneficiary.Name,
                Participation = companyBeneficiary.Participation,
            };
        }

        public static List<Beneficiary> CreateBeneficiarys(List<CompanyBeneficiary> companyBeneficiaries)
        {
            List<Beneficiary> beneficiary = new List<Beneficiary>();
            foreach (CompanyBeneficiary companyBeneficieres in companyBeneficiaries)
            {
                beneficiary.Add(CreateBeneficiary(companyBeneficieres));
            }
            return beneficiary;
        }

        public static Clause CreateClause(CompanyClause companyClause)
        {
            return new Clause
            {
                ConditionLevel = companyClause.ConditionLevel,
                ExtendedProperties = companyClause.ExtendedProperties,
                Id = companyClause.Id,
                IsMandatory = companyClause.IsMandatory,
                Name = companyClause.Name,
                Text = companyClause.Text,
                Title = companyClause.Title,
            };
        }

        public static List<Clause> CreateClauses(List<CompanyClause> companyClauses)
        {
            List<Clause> clause = new List<Clause>();
            foreach (CompanyClause companyBeneficieres in companyClauses)
            {
                clause.Add(CreateClause(companyBeneficieres));
            }
            return clause;
        }

        public static IssuanceIdentificationDocument CreateIssuanceIdentificationDocument(IdentificationDocument companyBeneficiary)
        {
            return new IssuanceIdentificationDocument
            {
                Number = companyBeneficiary.Number
            };
        }

        public static List<IssuanceIdentificationDocument> CreateIssuanceIdentificationDocumentss(List<IdentificationDocument> companyBeneficiarys)
        {
            List<IssuanceIdentificationDocument> issuanceIdentificationDocuments = new List<IssuanceIdentificationDocument>();
            foreach (IdentificationDocument companyBenficiary in companyBeneficiarys)
            {
                issuanceIdentificationDocuments.Add(CreateIssuanceIdentificationDocument(companyBenficiary));
            }
            return issuanceIdentificationDocuments;

        }

        public static CompanyVersion CreateCompanyVersion(Version version)
        {
            return new CompanyVersion
            {
                AirConditioning = version.AirConditioning,
                Body = new CompanyBody { Id = version.Body.Id },
                Currency = version.Currency,
                Description = version.Description,
                DoorQuantity = version.DoorQuantity,
                Engine = new CompanyEngine { EngineCc = version.Engine.EngineCc },
                ExtendedProperties = version.ExtendedProperties,
                Fuel = new CompanyFuel { Description = version.Fuel.Description },
                IaVehicleVersion = version.IaVehicleVersion,
                Id = version.Id,
                IsImported = version.IsImported,
                LastModel = version.LastModel,
                Make = new CompanyMake { Id = version.Make.Id },
                Model = new CompanyModel { Id = version.Model.Id },
                NewVehiclePrice = version.NewVehiclePrice,
                Novelty = version.Novelty,
                PartialLossBase = version.PartialLossBase,
                PassengerQuantity = version.PassengerQuantity,
                ServiceType = new CompanyServiceType { Id = version.ServiceType.Id },
                Status = version.Status,
                TonsQuantity = version.TonsQuantity,
                TransmissionType = new CompanyTransmissionType { Id = version.TransmissionType.Id },
                Type = new Vehicles.Models.CompanyType { Id = version.Type.Id },
                VehicleAxleQuantity = version.VehicleAxleQuantity,
                Weight = version.Weight,
                WeightCategory = version.WeightCategory,

            };
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }




    }
}
