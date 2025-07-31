using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyModificationController : ModificationController
    {
        public ActionResult CreateTemporal(ModificationViewModel modificationModel)
        {
            try
            {
                if (modificationModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["modificationModel.BusinessTypeDescription"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    modificationModel.UserId = SessionHelper.GetUserId();
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                    if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                    {
                        CompanyEndorsement.Text.TextBody = unicode_iso8859(CompanyEndorsement.Text.TextBody);
                    }
                    var policy = DelegateService.propertyModificationServiceCia.CreateTemporal(CompanyEndorsement, false);
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
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
        }
        private string unicode_iso8859(string text)
        {
            System.Text.Encoding iso = System.Text.Encoding.GetEncoding("iso8859-1");
            text = Regex.Replace(text, @"[/']", " ", RegexOptions.None);
            byte[] isoByte = iso.GetBytes(text);
            return iso.GetString(isoByte);
        }
        public ActionResult CreateTexts(ModificationViewModel modificationModel)
        {
            CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
            var UifJsonResult = new UifJsonResult(false, null);
            if (string.IsNullOrEmpty(modificationModel.Text))
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTextEmpty);
            }
            try
            {
                modificationModel.UserId = SessionHelper.GetUserId();
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                {
                    CompanyEndorsement.Text.TextBody = unicode_iso8859(CompanyEndorsement.Text.TextBody);
                }
                companyPolicyResult = DelegateService.propertyTextServiceCia.CreateTexts(CompanyEndorsement);
                if (companyPolicyResult.Errors != null && companyPolicyResult.Errors.Any())
                {
                    UifJsonResult = new UifJsonResult(companyPolicyResult.Errors.First().StateData, companyPolicyResult.Errors.First().Error);
                }
                else if (companyPolicyResult.InfringementPolicies != null && companyPolicyResult.InfringementPolicies.Count > 0)
                {
                    UifJsonResult = new UifJsonResult(true, new { companyPolicyResult.InfringementPolicies, companyPolicyResult.TemporalId });
                }
                else
                {
                    UifJsonResult = new UifJsonResult(true, companyPolicyResult);
                }
                return UifJsonResult;
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
                }

            }

        }
        public ActionResult CreateClauses(ModificationViewModel modificationModel)
        {
            try
            {
                var UifJsonResult = new UifJsonResult(false, null);
                modificationModel.UserId = SessionHelper.GetUserId();
                CompanyPolicy companyPolicy = ModelAssembler.CreateCompanyPolicy(modificationModel);
                CompanyPolicyResult companyPolicyResult = DelegateService.propertyClauseServiceCia.CreateClauses(companyPolicy);
                if (companyPolicyResult != null)
                {
                    if (companyPolicyResult.Errors != null && companyPolicyResult.Errors.Any())
                    {
                        UifJsonResult = new UifJsonResult(companyPolicyResult.Errors.First().StateData, companyPolicyResult.Errors.First().Error);
                    }
                    else if (companyPolicyResult.InfringementPolicies != null && companyPolicyResult.InfringementPolicies.Count > 0)
                    {
                        UifJsonResult = new UifJsonResult(true, new { companyPolicyResult.InfringementPolicies, companyPolicyResult.TemporalId });
                    }
                    else
                    {
                        UifJsonResult = new UifJsonResult(true, companyPolicyResult);
                    }
                }
                else
                {
                    UifJsonResult = new UifJsonResult(false, App_GlobalResources.Language.SelectAllRequiredClauses);
                }
                return UifJsonResult;
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
                }
            }


        }
    }
}