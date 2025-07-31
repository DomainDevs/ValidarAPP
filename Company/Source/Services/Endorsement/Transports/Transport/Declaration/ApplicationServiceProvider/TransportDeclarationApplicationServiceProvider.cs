using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Business;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Resources;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider
{
    public class TransportDeclarationApplicationServiceProvider : ITransportDeclarationApplicationService
    {
        /// <summary>
        /// crear la declaration
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        public DeclarationDTO CreateDeclaration(DeclarationDTO declarationDTO)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateDeclaration(declarationDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// calculo de la declaration
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        public DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.QuotateDeclaration(declarationDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// Consulta información de la póliza
        /// <param name="prefixId" name="branchId" name="policyNumber"></param>
        /// </summary>
        public DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// consulta los riegos
        /// <param name="policyId">Modelo declaracion</param>
        /// </summary>
        public DeclarationDTO GetTransportsByPolicyId(int PolicyId,string currentFrom)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportsByPolicyId(PolicyId,currentFrom);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetInsuredObjectsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetInsuredObjects), ex);
            }
        }

        /// <summary>
        /// consulta de endosos por tipo y poliza
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        
        public DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, PolicyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// consulta el riesgo
        /// <param name="declarationDTO">Modelo declaracion</param>
        /// </summary>
        
        public DeclarationDTO GetTransportByRiskId(TransportDTO Risk)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportByRiskId(Risk);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// create Endorsement Declaration
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateEndorsementDeclaration(companyPolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO CreateTemporal(DeclarationDTO declarationtDTO)
        {
            try
            {
                TransportBusiness declarationTransportBusiness = new TransportBusiness();
                return declarationTransportBusiness.CreateTemporal(declarationtDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetTemporalById(int temporalId, bool isMasive)
        {
            try
            {
                TransportBusiness declarationTransportBusiness = new TransportBusiness();
                return declarationTransportBusiness.GetTemporalById(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeclaration), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
		public DeclarationDTO GetDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetDeclarationEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDeclarationEndorsementByPolicyId), ex);
            }
        }
    }
}