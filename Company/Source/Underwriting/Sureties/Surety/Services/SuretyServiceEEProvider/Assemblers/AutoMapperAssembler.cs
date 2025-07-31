using AutoMapper;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Sistran.Company.Application.Issuance.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Contracts Risk
        public static IMapper CreateMapRiskPostS()
        {
            IMapper config = MapperCache.GetMapper<ENT.PrvRiskCoveragePost, CompanyRiskSuretyPost>(cfg =>
            {
                cfg.CreateMap<ENT.PrvRiskCoveragePost, CompanyRiskSuretyPost>()
                 .ForMember(d => d.ChkContractDate, o => o.MapFrom(c => ValidatecompanyRiskSuretyPost(c).ChkContractDate))
                 .ForMember(d => d.ContractDate, o => o.MapFrom(c => ValidatecompanyRiskSuretyPost(c).ContractDate))
                 .ForMember(d => d.ChkContractFinalyDate, o => o.MapFrom(c => ValidatecompanyRiskSuretyPost(c).ContractDate))
                 .ForMember(d => d.IssueDate, o => o.MapFrom(c => ValidatecompanyRiskSuretyPost(c).IssueDate));

            });
            return config;
        }

        static CompanyRiskSuretyPost ValidatecompanyRiskSuretyPost(ENT.PrvRiskCoveragePost prvRiskCoveragePost)
        {
            CompanyRiskSuretyPost companyTempRiskSuretyPost = new CompanyRiskSuretyPost();
            if (prvRiskCoveragePost.ContractDate != null)
            {
                companyTempRiskSuretyPost.ChkContractDate = true;
                companyTempRiskSuretyPost.ContractDate = prvRiskCoveragePost.ContractDate;
            }
            if (prvRiskCoveragePost.DeliveryDate != null)
            {
                companyTempRiskSuretyPost.ChkContractFinalyDate = true;
                companyTempRiskSuretyPost.IssueDate = prvRiskCoveragePost.DeliveryDate;
            }

            return companyTempRiskSuretyPost;
        }
        #endregion

        #region Beneficiary
        public static IMapper CreateMapBeneficiary()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.RiskBeneficiary, CompanyBeneficiary>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskBeneficiary, CompanyBeneficiary>()
                 .ForMember(d => d.IndividualId, o => o.MapFrom(c => c.BeneficiaryId))
                 .ForMember(d => d.CustomerType, o => o.MapFrom(c => CustomerType.Individual))
                 .ForMember(d => d.BeneficiaryType, o => o.MapFrom(c => new CompanyBeneficiaryType
                 {
                     Id = c.BeneficiaryTypeCode
                 }))
                 .ForMember(d => d.Participation, o => o.MapFrom(c => c.BenefitPercentage))
                 .ForMember(d => d.IndividualType, o => o.MapFrom(c => c.BeneficiaryTypeCode == (int)IndividualType.Person ? IndividualType.Person : IndividualType.Company))
                  .ForMember(d => d.CompanyName, o => o.MapFrom(c => new IssuanceCompanyName
                  {
                      NameNum = c.NameNum.GetValueOrDefault(),
                      Address = new IssuanceAddress
                      {
                          Id = c.AddressId
                      }
                  }));
            });
            return config;
        }

        public static IMapper CreateMapDynamicConcept()
        {
            IMapper config = MapperCache.GetMapper<Rules.Concept, DynamicConcept>(cfg =>
            {
                cfg.CreateMap<Rules.Concept, DynamicConcept>();
            });
            return config;
        }

        public static IMapper CreateMapBeneficiaryFromInsured()
        {
            IMapper config = MapperCache.GetMapper<CompanyIssuanceInsured, CompanyBeneficiary>(cfg =>
            {
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>()
            .ForMember(d => d.Participation, o => o.MapFrom(c => CommisionValue.Participation))
            .ForMember(d => d.BeneficiaryType, o => o.MapFrom(c => new CompanyBeneficiaryType
            {
                Id = KeySettings.OnerousBeneficiaryTypeId,
                SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription
            }))
            .ForMember(d => d.BeneficiaryTypeDescription, o => o.MapFrom(c => companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription));
            });
            return config;
        }

        #endregion


    }
}
