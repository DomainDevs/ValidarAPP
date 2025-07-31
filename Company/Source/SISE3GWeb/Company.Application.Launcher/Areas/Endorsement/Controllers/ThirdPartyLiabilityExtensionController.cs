using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ThirdPartyLiabilityExtensionController : ExtensionController
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
                    if (!string.IsNullOrEmpty(companyPolicy.Endorsement.Text.TextBody))
                        companyPolicy.Endorsement.Text.TextBody = unicode_iso8859(companyPolicy.Endorsement.Text.TextBody);
                    var policy = DelegateService.thirdPartyLiabilityExtensionServiceCia.CreateExtension(companyPolicy);
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

    }
}