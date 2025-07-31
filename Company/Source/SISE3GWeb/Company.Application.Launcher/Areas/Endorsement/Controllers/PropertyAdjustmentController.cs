using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyAdjustmentController : EndorsementBaseController
    {
        // GET: Endorsement/PropertyAdjustment
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                var policy = DelegateService.propertyAdjustmentApplicationService.GetRisksByTemporalId(temporalId, false);
                return new UifJsonResult(true, policy);

            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                AdjustmentDTO adjustment = DelegateService.propertyAdjustmentApplicationService.GetAdjustmentEndorsementByPolicyId(policyId);
                return new UifJsonResult(true, adjustment);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAdjustmentEndorsementByPolicyId);
            }
        }

        public ActionResult GetRisksByPolicyId(int policyId, string currentFrom)
        {
            try
            {
                List<CompanyPropertyRisk> risks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(policyId);
                if (risks != null)
                {
                    return new UifJsonResult(true, risks.Select(x => new
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
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult CalculateDays(string inputFrom, string inputTo, int billingPeriodId)
        {
            try
            {
                AdjustmentDTO endorsement = DelegateService.propertyAdjustmentApplicationService.CalculateDays(inputFrom, inputTo, billingPeriodId);
                return new UifJsonResult(true, endorsement);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorCalculateDays);
            }

        }

        public ActionResult GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            try
            {
                AdjustmentDetailsDTO adjustmentDetails = DelegateService.propertyAdjustmentApplicationService.GetEndorsementByEndorsementTypeDeclarationPolicyId(policyId, riskId);

                return new UifJsonResult(true, adjustmentDetails.DeclarationEndorsements);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorGetCoverage);
            }

        }

        public ActionResult CreateTemporal(AdjustmentDTO adjustmentModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adjustmentModel.UserId = SessionHelper.GetUserId();
                    var policy = DelegateService.propertyAdjustmentApplicationService.CreateTemporal(adjustmentModel);
                    if (policy != null)
                    {
                        return new UifJsonResult(true, policy);
                    }
                    else
                    {
                        return new UifJsonResult(false, "");
                    }
                }
                else
                {
                    return new UifJsonResult(false, "");
                }
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();
                insuredObjects = DelegateService.propertyAdjustmentApplicationService.GetInsuredObjectsByRiskId(riskId);
                return new UifJsonResult(true, insuredObjects.Where(x => x.IsDeclarative == true));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorGetInsuredObject);
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
                //bool canMakeAdjustmentEndorsement = DelegateService.propertyService.CanMakeAdjustmentEndorsement(policyId);
                //return new UifJsonResult(true, canMakeAdjustmentEndorsement);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, false);
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
        public ActionResult EndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId)
        {
            try
            {
                bool isAllow = DelegateService.propertyService.CanMakeEndorsementByRiskByInsuredObjectId(policyId, riskId, insuredObjectId, EndorsementType.AdjustmentEndorsement);
                return new UifJsonResult(true, isAllow);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
            }
        }

    }
}