using Sistran.Company.Application.DeclarationApplicationService;
using Sistran.Company.Application.DeclarationApplicationService.DTO;
using Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Business;
using Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.DeclarationApplicationServiceEEProvider
{
    public class DeclarationApplicationEEProvider : IDeclarationApplicationService
    {
        public DeclarationDTO CreateTemporal(DeclarationDTO declarationDTO)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.CreateTemporal(declarationDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, PolicyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO GetDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetDeclarationEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorGetDeclarationEndorsementByPolicyId), ex);
            }
        }

        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetInsuredObjectsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorGetInsuredObjects), ex);
            }
        }

        public DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO GetTemporalById(int temporalId, bool isMasive)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetTemporalById(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO GetRiskByRiskId(CompanyPropertyRisk Risk)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetRiskByRiskId(Risk);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO GetRisksByPolicyIdCurrentFrom(int PolicyId, string currentFrom)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetRisksByPolicyIdCurrentFrom(PolicyId, currentFrom);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorCreateDeclaration), ex);
            }
        }

        public DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO)
        {
            throw new NotImplementedException();
        }

        public List<CompanyPropertyRisk> GetRisksByPolicyId(int policyId)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.GetRisksByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Error.ErrorGetRisksByPolicyId), ex);
            }
        }

        public bool ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            try
            {
                DeclarationPropertyBusiness declarationPropertyBusiness = new DeclarationPropertyBusiness();
                return declarationPropertyBusiness.ValidateDeclarativeInsuredObjects(policyId);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
