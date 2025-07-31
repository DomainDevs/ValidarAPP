using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyRenewalController : RenewalController
    {
        public ActionResult CreateTemporal(RenewalViewModel renewalModel)
        {
            try
            {
                int id = Convert.ToInt32(renewalModel.EndorsementId);
                int UserId = SessionHelper.GetUserId();

                if (DelegateService.utilitiesServiceCore.GetEndorsementControlById(id, UserId))
                {
                    var CompanyPolicy = ModelAssembler.CreateCompanyPolicyByRenewal(renewalModel);
                    CompanyPolicy.Id = renewalModel.TemporalId == null ? 0: renewalModel.TemporalId.Value;
                    CompanyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                    if (CompanyPolicy.BeginDate < DateHelper.GetMinDate())
                    {
                        CompanyPolicy.BeginDate = DateHelper.GetMinDate();
                    }
                    if (CompanyPolicy.CurrentTo < DateHelper.GetMinDate())
                    {
                        CompanyPolicy.CurrentTo = DateHelper.GetMinDate();
                    }
                    if (CompanyPolicy.CurrentFrom < DateHelper.GetMinDate())
                    {
                        CompanyPolicy.CurrentFrom = DateHelper.GetMinDate();
                    }
                    if (CompanyPolicy.Endorsement.CurrentTo < DateHelper.GetMinDate())
                    {
                        CompanyPolicy.Endorsement.CurrentTo = DateHelper.GetMinDate();
                    }
                    if (CompanyPolicy.Endorsement.CurrentFrom < DateHelper.GetMinDate())
                    {
                        CompanyPolicy.Endorsement.CurrentFrom = DateHelper.GetMinDate();
                    }

                    if (!string.IsNullOrEmpty(CompanyPolicy.Endorsement.Text.TextBody))
                        CompanyPolicy.Endorsement.Text.TextBody = unicode_iso8859(CompanyPolicy.Endorsement.Text.TextBody);
                    CompanyPolicy.UserId = UserId;
                    var policy = DelegateService.suretyRenewalServiceCia.CreateRenewal(CompanyPolicy);
                    if (policy != null)
                    {
                        return new UifJsonResult(true, policy);
                    }
                    else
                    {
                        string errorMessage = GetErrorMessages();
                        return new UifJsonResult(false, errorMessage);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyInEdition);
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