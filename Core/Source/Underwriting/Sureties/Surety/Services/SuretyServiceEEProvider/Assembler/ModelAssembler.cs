using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
using System;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Models;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Assembler
{
    public class ModelAssembler
    {
        /// <summary>
        /// Crear Riesgo de cumplimiento
        /// </summary>
        /// <param name="entityRisk">entidad risk</param>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns></returns>
        public static Contract CreateContract(ContractMapper contractMapper)
        {
            var config = AutoMapperAssembler.CreateMapContract();
            var mapperPolicy = config.Map<ContractMapper, Contract>(contractMapper);
            return mapperPolicy;
            //return new Contract
            //{
            //    Risk = new Risk
            //    {
                    //RiskId = entityRisk.RiskId,
                    //CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    //GroupCoverage = new GroupCoverage
                    //{
                    //    Id = entityRisk.CoverGroupId.Value,
                    //    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode
                    //},
                    //MainInsured = new IssuanceInsured
                    //{
                    //    IndividualId = entityRisk.InsuredId,
                    //    CompanyName = new IssuanceCompanyName
                    //    {
                    //        NameNum = entityRisk.NameNum.GetValueOrDefault(),
                    //        Address = new IssuanceAddress
                    //        {
                    //            Id = entityRisk.AddressId.GetValueOrDefault()
                    //        }
                    //    }
                    //},
                    //Policy = new Policy
                    //{
                    //    Id = entityEndorsementRisk.PolicyId,
                    //    DocumentNumber = entityPolicy.DocumentNumber,
                    //    Endorsement = new Endorsement { Id = entityEndorsementRisk.EndorsementId, PolicyId = entityPolicy.PolicyId },
                    //},
                    //Number = entityEndorsementRisk.RiskNum,
                    //OriginalStatus = (RiskStatusType)entityEndorsementRisk.RiskStatusCode,
                    //Status = RiskStatusType.NotModified,
                    //DynamicProperties = new List<DynamicConcept>()
                //},
                //Value = new CommonService.Models.Amount
                //{
                //    Value = entityRiskSurety.ContractAmount
                //},
                //ContractType = new ContractType
                //{
                //    Id = entityRiskSurety.SuretyContractTypeCode
                //},
                //Isfacultative = entityRiskSurety.IsFacultative,
                //Class = new ContractClass
                //{
                //    Id = Convert.ToInt32(entityRiskSurety.SuretyContractCategoriesCode)
                //},
            //    Contractor = new Contractor
            //    {
            //        IndividualId = entityRiskSurety.IndividualId
            //    },
            //    SettledNumber = entityRiskSurety.BidNumber,
            //    ContractObject = new Text
            //    {
            //        TextBody = entityRiskSuretyContract.ObjectContract
            //    }
            //};
        }
    }
}
