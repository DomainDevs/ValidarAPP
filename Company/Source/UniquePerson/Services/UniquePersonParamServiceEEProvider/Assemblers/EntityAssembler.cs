// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers
{

    using Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using compUniquePersonModels = Sistran.Company.Application.UniquePersonParamService.Models;
    

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class EntityAssembler
    {
        /// <summary>
        /// Construye la entidad de Firma representante legal
        /// </summary>
        /// <param name="paramLegalRepresentativeSing">Modelo de Firma representante legal</param>
        /// <returns>Entidad de Firma representante legal</returns>
        public static CptLegalReprSign CreateCptLegalReprSign(ParamLegalRepresentativeSing paramLegalRepresentativeSing)
        {
            return new CptLegalReprSign(paramLegalRepresentativeSing.ParamCompanyType.Id, paramLegalRepresentativeSing.ParamBranchType.Id, paramLegalRepresentativeSing.CurrentFrom)
            {
                LegalRepresentative = paramLegalRepresentativeSing.LegalRepresentative,
                PathSignatureImg = paramLegalRepresentativeSing.PathSignatureImg,
                SignatureImg = paramLegalRepresentativeSing.SignatureImg,
                UserId = paramLegalRepresentativeSing.UserId
            };
        }

        /// <summary>
        /// Transfomra desde modelo de negocio a entidad de base de datos
        /// </summary>
        /// <param name="branchAlliance">Modelo de sucursal</param>
        /// <returns>Entidad de punto de sucursal</returns>
        public static CptBranchAlliance CreateBranchAlliance(BranchAlliance branchAlliance)
        {
            return new CptBranchAlliance(branchAlliance.BranchId, branchAlliance.AllianceId)
            {
                BranchDescription = branchAlliance.BranchDescription,
                CityCD = branchAlliance.CityCD,
                StateCD = branchAlliance.StateCD,
                CountryCD = branchAlliance.CountryCD
            };
        }

        /// <summary>
        /// Transfomra desde modelo de negocio a entidad de base de datos
        /// </summary>
        /// <param name="alliance">Modelo de negocio</param>
        /// <returns>Entidad de base de datos</returns>
        public static CptAlliance CreateAlliance(compUniquePersonModels.Alliance alliance)
        {
            return new CptAlliance(alliance.AllianceId)
            {
                Description = alliance.Description,
                IsScore = alliance.IsScore,
                IsFine = alliance.IsFine
            };
        }

        /// <summary>
        /// Transfomra desde modelo de negocio a entidad de base de datos
        /// </summary>
        /// <param name="salePoint">Modelo de punto de venta</param>
        /// <returns>Entidad de punto de venta</returns>
        public static CptAllianceBranchSalePoint CreateSalePointAlliance(compUniquePersonModels.AllianceBranchSalePonit salePoint)
        {
            return new CptAllianceBranchSalePoint(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId)
            {
                SalePointDescription = salePoint.SalePointDescription
            };
        }

        //public static CptAgentAlliance CreateAgentAgency(modelsUPersonCore.AgentAgency agentAgency)
        //{
        //    return new CptAgentAlliance(agentAgency.IndividualId, agentAgency.AgencyAgencyId, agentAgency.AllianceId)
        //    {
        //        SpecialPrint = agentAgency.IsSpecialImpression
        //    };
        //}
    }
}
