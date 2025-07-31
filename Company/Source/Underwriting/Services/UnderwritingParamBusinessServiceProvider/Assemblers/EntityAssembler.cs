// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers
{
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.Common.Entities;

    /// <summary>
    /// EntityAssembler. Proveedor del servicio de aplicación.
    /// </summary>
    public class EntityAssembler
    {
        public static MinPremiumRelation CreateEntity(CompanyParamMinPremiunRelation companyParam)
        {
            return new MinPremiumRelation()
            {
                BranchCode = companyParam.Branch.Id,
                CurrencyCode = companyParam.Currency.Id,
                EndoTypeCode = companyParam.EndorsementType.Id,
                Key1 = companyParam.Product.Id,
                Key2 = (companyParam.GroupCoverage != null ? companyParam.GroupCoverage.Id : (companyParam.MinPremiunRange != null ? companyParam.MinPremiunRange.Id : 0)),
                //MinPremiumRelId = companyParam.MinPremiunRange.Id,
                PrefixCode = companyParam.Prefix.Id,
                RiskMinPremium = companyParam.RiskMinPremiun,
                SubsMinPremium = companyParam.SubMinPremiun,
                CalculateProrate = false
            };
        }
    }
}
