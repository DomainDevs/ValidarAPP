using AutoMapper;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Linq;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region automapper
        #region Clausulas
        public static IMapper CreateMapCompanyClause()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config.CreateMapper();
        }
        #endregion Clausula
        #endregion

        public static CompanyEndorsementDetail CreateCompanyEndorsementDetail(CompanyPolicy policy, CompanyTransport companyTransport)
        {
            CompanyEndorsementDetail companyEndorsementDetail = new CompanyEndorsementDetail()
            {
                PolicyId = policy.DocumentNumber,
                EndorsementType = (int)policy.Endorsement.EndorsementType,
                RiskNum = companyTransport.Risk.RiskId,
                InsuredObjectId = (int)policy.Endorsement.InsuredObjectId,
                DeclarationValue = policy.Endorsement.DeclaredValue,
            };
            CompanyInsuredObject currentInsured = companyTransport.InsuredObjects.Where(x => x.Id.Equals(policy.Endorsement.InsuredObjectId)).FirstOrDefault();
            if (currentInsured.DepositPremiunPercent > 0)
            {
                companyEndorsementDetail.PremiumAmount = (policy.Endorsement.DeclaredValue * currentInsured.DepositPremiunPercent) * currentInsured.Rate;
            }
            else
            {
                companyEndorsementDetail.PremiumAmount = (policy.Endorsement.DeclaredValue * currentInsured.Rate);
            }

            return companyEndorsementDetail;

        }
    }
}