using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.DeclarationApplicationService.DTO;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyDeclarationController : EndorsementBaseController
    {
        private string message = string.Empty;

        public ActionResult GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();
                insuredObjects = DelegateService.propertyDeclarationApplicationService.GetInsuredObjectsByRiskId(riskId);
                return new UifJsonResult(true, insuredObjects.Where(x => x.IsDeclarative == true));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCalculateDays);
            }
        }
        public ActionResult ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            try
            {
                bool result = DelegateService.propertyDeclarationApplicationService.ValidateDeclarativeInsuredObjects(policyId);
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInsuredObject);
            }
        }

        public ActionResult GetRisksByPolicyId(int policyId)
        {
            try
            {
                List<CompanyPropertyRisk> propertyRisks = DelegateService.propertyDeclarationApplicationService.GetRisksByPolicyId(policyId);
                if (propertyRisks != null)
                {
                    return new UifJsonResult(true, propertyRisks.Select(x => new
                    {
                        RiskId = x.Risk.RiskId,
                        Description = x.FullAddress,
                        CoverageGroupId = x.Risk.GroupCoverage.Id
                    }).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTransportRisksByPolicyId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetRiskByRiskId(CompanyPropertyRisk Risk)
        {
            try
            {
                DeclarationDTO policy = DelegateService.propertyDeclarationApplicationService.GetRiskByRiskId(Risk);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CreateTemporal(DeclarationDTO declarationModel)
        {
            try
            {
                declarationModel.UserId = SessionHelper.GetUserId();
                var policy = DelegateService.propertyDeclarationApplicationService.CreateTemporal(declarationModel);
                return new UifJsonResult(true, policy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        private CompanyPolicy GetPolicyDescriptions(CompanyPolicy policy)
        {
            var iMapper = ModelAssembler.CreateMapBranch();
            if (policy.Branch != null && string.IsNullOrEmpty(policy.Branch.Description))
            {
                policy.Branch = iMapper.Map<Application.CommonService.Models.Branch, CompanyBranch>(DelegateService.commonService.GetBranchById(policy.Branch.Id));
            }
            var iMapperPrefix = ModelAssembler.CreateMapPrefix();
            if (string.IsNullOrEmpty(policy.Prefix.Description))
            {
                policy.Prefix = iMapperPrefix.Map<Application.CommonService.Models.Prefix, CompanyPrefix>(DelegateService.commonService.GetPrefixById(policy.Prefix.Id));
            }

            if (string.IsNullOrEmpty(policy.Product.Description))
            {
                policy.Product = DelegateService.productService.GetCompanyProductById(policy.Product.Id);
            }

            if (policy.Endorsement.Text == null)
            {
                policy.Endorsement.Text = policy.Text;
            }

            return policy;
        }

        private string ValidateEndorsement(int temporalId)
        {
            string message = string.Empty;
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy != null)
                {

                    if (policy.Endorsement.EndorsementType != EndorsementType.Cancellation && policy.Endorsement.EndorsementType != EndorsementType.Nominative_cancellation)
                    {
                        List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);

                        if (risks.Count > 0)
                        {
                            bool validEndorsement = true;
                            foreach (CompanyRisk risk in risks)
                            {

                                if (policy.PolicyOrigin == PolicyOrigin.Collective)
                                {
                                    if (risk.Status != null)
                                    {
                                        validEndorsement = false;
                                        message = string.Empty;
                                    }
                                }

                                if (risk.Status != null && risk.Status.Value != RiskStatusType.Excluded)
                                {
                                    validEndorsement = false;
                                    message = string.Empty;
                                }
                                switch (policy.Endorsement.EndorsementType)
                                {
                                    case EndorsementType.Modification:
                                        switch (policy.Product.CoveredRisk.CoveredRiskType)
                                        {
                                            case CoveredRiskType.Surety:
                                                message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateSurety);
                                                break;

                                            default:
                                                message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateDefault);
                                                break;

                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (validEndorsement)
                            {
                                message = App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                            }

                            return message;
                        }
                        else
                        {
                            return App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return App_GlobalResources.Language.NoCanEndorsementsWithoutRisks;
                }
            }
            catch (Exception)
            {
                return App_GlobalResources.Language.ErrorValideEndorsement;
            }
        }

        private string ValidateDate(CompanyRisk risk, CompanyPolicy policy, ValidateTyeEndorsement validateTyeEndorsement)
        {
            message = string.Empty;
            DateTime? CurrentFrom = null;
            DateTime? CurrentTo = null;
            var query = new List<CompanyCoverage>();
            switch (validateTyeEndorsement)
            {

                case ValidateTyeEndorsement.DateSurety:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (policy.CurrentTo < CurrentTo)
                        {
                            message = App_GlobalResources.Language.ErrorDatesCoveragesPolicy;
                        }
                    }
                    if (CurrentFrom != null)
                    {
                        if (CurrentFrom < policy.CurrentFrom)
                        {
                            message = App_GlobalResources.Language.ErrorDatesFromCoveragesPolicy;
                        }
                    }
                    break;
                case ValidateTyeEndorsement.DateDefault:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (CurrentTo > policy.CurrentTo)
                        {
                            message = App_GlobalResources.Language.ErrorDatesCoverageGreaterEndorsement;
                        }
                    }
                    if (CurrentFrom != null)
                    {
                        if (CurrentFrom > policy.CurrentFrom)
                        {
                            message = App_GlobalResources.Language.ErrorDatesFromCoverageGreaterEndorsement;
                        }
                    }
                    break;
                default:
                    break;

            }
            return message;
        }

        enum ValidateTyeEndorsement
        {
            DateSurety = 1,
            DateDefault = 2
        }

        private List<PoliciesAut> ValidateInfringementPolicies(CompanyPolicy policy)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            if (policy != null)
            {

                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count > 0)
                {
                    infringementPolicies.AddRange(policy.InfringementPolicies.Where(x => x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive).ToList());
                }

                List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(policy.Id, false);

                foreach (CompanyRisk risk in risks)
                {

                    if (risk.InfringementPolicies != null && risk.InfringementPolicies.Count > 0)
                    {
                        infringementPolicies.AddRange(risk.InfringementPolicies.Where(x => x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Authorization || x.Type == Application.AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive).ToList());
                    }
                }
            }

            return infringementPolicies;
        }

        public ActionResult GetTemporalId(int temporalId)
        {
            try
            {
                var policy = DelegateService.propertyDeclarationApplicationService.GetTemporalById(temporalId, false);
                return new UifJsonResult(true, policy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                DeclarationDTO declaration = DelegateService.propertyDeclarationApplicationService.GetDeclarationEndorsementByPolicyId(policyId);
                return new UifJsonResult(true, declaration);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeclarationEndorsementByPolicyId);
            }
        }

        public ActionResult CanMakeEndorsement(int policyId)
        {
            try
            {
                Dictionary<string, object> resulValidation = new Dictionary<string, object>();
                var makeEndrsement = DelegateService.propertyService.CanMakeEndorsement(policyId, out resulValidation);
                resulValidation.Add("CanMakeEndorsement", makeEndrsement);
                return new UifJsonResult(true, resulValidation);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, false);
            }
        }
        public ActionResult EndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId)
        {
            try
            {
                bool isAllow = DelegateService.propertyService.CanMakeEndorsementByRiskByInsuredObjectId(policyId, riskId, insuredObjectId, EndorsementType.DeclarationEndorsement);
                return new UifJsonResult(true, isAllow);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
            }
        }
    }
}