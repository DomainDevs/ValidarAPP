using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sistran.Core.Application.UniquePerson.IntegrationService.Models;
using Sistran.Core.Application.UniquePerson.IntegrationService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Provider.Assemblers
{
    public static class IntegrationAssembler
    {
        internal static IntegrationAgency CreateIntegrationAgency(Agency agency)
        {
            return new IntegrationAgency
            {
                Id = agency.Id,
                Code = agency.Code,
                FullName = agency.FullName,
                DeclinedDate = agency.DateDeclined,
                Agent = new IntegrationAgent
                {
                    Id = agency.Agent.IndividualId,
                    FullName = agency.Agent.FullName,
                    DeclineDate = agency.Agent.DateDeclined
                }
            };
        }

        internal static IntegrationAgent CreateIntegrationAgent(Agent agent)
        {
            return new IntegrationAgent
            {
                Id = agent.IndividualId,
                FullName = agent.FullName,
                DeclineDate = agent.DateDeclined,
            };
        }

        internal static IntegrationInsured CreateIntegrationInsured(Insured insured)
        {
            return new IntegrationInsured
            {
                IndividualId = insured.IndividualId,
                FullName  = insured.FullName,
                IdentificationNumber = insured.IdentificationDocument,
                //IdentificationNumber = insured.IdentificationDocument.Number,
                //IdentificationTypeId = insured.IdentificationDocument.DocumentType.Id,
                //Code = insured.InsuredId,
                //FullName = insured.IndividualType == IndividualType.Person ? insured.SurName + " " + (string.IsNullOrEmpty(insured.SecondSurName) ? "" : insured.SecondSurName + " ") + insured.Name : insured.Name,
                //BirthDate = Convert.ToDateTime(insured.BirthDate),
                CustomerType = insured.CustomerType,
                DeclinedDate = insured.DeclinedDate,
                ///  PaymentId = insured.PaymentMethod != null  ? insured.PaymentMethod.PaymentId : 0,
            };

        }

        internal static List<IntegrationInsured> CreateIntegrationInsuredList(List<Insured> insuredList)
        {
            List<IntegrationInsured> returnList = new List<IntegrationInsured>();

            foreach (var insured in insuredList)
            {
                returnList.Add(CreateIntegrationInsured(insured));
            }

            return returnList;
        }

        internal static CompanyDTO CreateCompanyByIndividualId(Company company)
        {
            return new CompanyDTO() 
            {
                AssociationType = new AssociationTypeDTO 
                {
                    Id = company.AssociationType == null ? 0 : company.AssociationType.Id,
                    Description = company.AssociationType ==  null ? "" : company.AssociationType.Description
                },
                CompanyType = new CompanyTypeDTO 
                { 
                    Id = company.CompanyType == null ? 0 : company.CompanyType.Id,
                    Description = company.CompanyType == null ? "" : company.CompanyType.Description,
                },
                IndividualId = company.IndividualId,
                FullName = company.FullName,
                Insured = new InsuredDTO() 
                { 
                    Agency = new AgencyDTO() 
                    {
                        Id = company.Insured == null ? 0 : company.Insured.Agency == null ? 0 : company.Insured.Agency.Id,
                        FullName = company.Insured == null ? "" : company.Insured.Agency == null ? "" : company.Insured.Agency.FullName,
                    }
                }
            };
        }

        internal static IMapper CreateMapEconomicGroupIntegration()
        {
            var config = MapperCache.GetMapper<EconomicGroup, IntegrationEconomicGroup>(cfg =>
            {
                cfg.CreateMap<EconomicGroup, IntegrationEconomicGroup>();
                cfg.CreateMap<BaseEconomicGroup, BaseIntegrationEconomicGroup>();
                cfg.CreateMap<EconomicGroupDetail, IntegrationEconomicGroupDetail>();
                cfg.CreateMap<BaseEconomicGroupDetail, BaseIntegrationEconomicGroupDetail>();
            });
            return config;
        }

        internal static IntegrationEconomicGroup ToIntegrationDTO(this EconomicGroup economicGroup)
        {
            var config = CreateMapEconomicGroupIntegration();
            return config.Map<EconomicGroup, IntegrationEconomicGroup>(economicGroup);
        }

        internal static IEnumerable<IntegrationEconomicGroup> ToIntegrationDTOs(this IEnumerable<EconomicGroup> economicGroupEvents)
        {
            return economicGroupEvents.Select(ToIntegrationDTO);
        }

        internal static ReinsurerDTO CreateIntegrationReinsurerDTO(ReInsurer reinsurer)
        {
            return new ReinsurerDTO
            {
                IndividualId = reinsurer.IndividualId,
                ReinsuredCD = reinsurer.ReinsuredCD,
                ModifyDate = reinsurer.ModifyDate,
                EnteredDate = reinsurer.EnteredDate,
                DeclinedDate = reinsurer.DeclinedDate,
                DeclaredTypeCD = reinsurer.DeclaredTypeCD,
                Annotations = reinsurer.Annotations
            };
        }

        internal static IMapper CreateMapEconomicGroupDetailIntegration()
        {
            var config = MapperCache.GetMapper<EconomicGroup, IntegrationEconomicGroup>(cfg =>
            {
                cfg.CreateMap<EconomicGroupDetail, IntegrationEconomicGroupDetail>();
                cfg.CreateMap<BaseEconomicGroupDetail, BaseIntegrationEconomicGroupDetail>();
            });
            return config;
        }

        internal static IntegrationEconomicGroupDetail ToIntegrationDTO(this EconomicGroupDetail economicGroupDetail)
        {
            var config = CreateMapEconomicGroupDetailIntegration();
            return config.Map<EconomicGroupDetail, IntegrationEconomicGroupDetail>(economicGroupDetail);
        }

        internal static IEnumerable<IntegrationEconomicGroupDetail> ToIntegrationDTOs(this IEnumerable<EconomicGroupDetail> economicGroupDetails)
        {
            return economicGroupDetails.Select(ToIntegrationDTO);
        }

    }
}