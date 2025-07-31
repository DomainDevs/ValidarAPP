using AutoMapper;
using TRMOD = Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Quotation.Entities;

using System.Collections.Generic;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
           /// <summary>
        /// Creates the company coverages.
        /// </summary>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        public static CompanyCoverage CreateCompanyCoverage(RiskCoverage coverages)
        {
            return new CompanyCoverage()
            {
                DeclaredAmount = coverages.DeclaredAmount,
                PremiumAmount = coverages.PremiumAmount,
                IsMinPremiumDeposit = coverages.IsMinPremiumDeposit,
                LimitAmount = coverages.LimitAmount
            };
          
        }

        public static TRMOD.CompanyEndorsementDetail CreateCompanyEndorsementDetail() {
            return new TRMOD.CompanyEndorsementDetail { 
                
                };
            }


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
            //CompanyInsuredObject currentInsured = companyTransport.InsuredObjects.Where(x => x.Id.Equals(policy.Endorsement.InsuredObjectId)).FirstOrDefault();
            //if (currentInsured.DepositPremiunPercent > 0)
            //{
            //    companyEndorsementDetail.PremiumAmmount = (policy.Endorsement.DeclaredValue * currentInsured.DepositPremiunPercent) * currentInsured.Rate;
            //}
            //else
            //{
            //    companyEndorsementDetail.PremiumAmmount = (policy.Endorsement.DeclaredValue * currentInsured.Rate);
            //}

            return companyEndorsementDetail;

        }

        #region automapper

        public static IMapper CreateMapCompanyClause()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });

            return config.CreateMapper();
        }

        #endregion
    }
}