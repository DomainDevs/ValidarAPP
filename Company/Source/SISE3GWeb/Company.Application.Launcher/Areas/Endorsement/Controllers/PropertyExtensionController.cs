using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyExtensionController : ExtensionController
    {
        public ActionResult CreateTemporal(ExtensionViewModel extensionModel)
        {
            try
            {
                if (extensionModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["extensionModel.BusinessTypeDescription"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    extensionModel.UserId = SessionHelper.GetUserId();
                    var companyPolicy = ModelAssembler.CreateCompanyEndorsement(extensionModel);
                    var policy = DelegateService.propertyExtensionServiceCia.CreateExtension(companyPolicy);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
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
        public ActionResult GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();
                insuredObjects = DelegateService.propertyService.GetInsuredObjectsByRiskId(riskId);
                return new UifJsonResult(true, insuredObjects.Where(x => x.IsDeclarative == true));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredObject);
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
    }
}