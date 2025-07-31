using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class TransportRenewalController : RenewalController
    {

        public ActionResult CreateTemporal(RenewalViewModel renewalModel)
        {
            try
            {

                CompanyPolicy companyPolicy = ModelAssembler.CreateCompanyPolicyByRenewal(renewalModel);
                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                if (companyPolicy.BeginDate <  DateHelper.GetMinDate())
                {
                    companyPolicy.BeginDate =  DateHelper.GetMinDate();
                }
                if (companyPolicy.CurrentTo <  DateHelper.GetMinDate())
                {
                    companyPolicy.CurrentTo =  DateHelper.GetMinDate();
                }
                if (companyPolicy.CurrentFrom <  DateHelper.GetMinDate())
                {
                    companyPolicy.CurrentFrom =  DateHelper.GetMinDate();
                }
                if (companyPolicy.Endorsement.CurrentTo <  DateHelper.GetMinDate())
                {
                    companyPolicy.Endorsement.CurrentTo =  DateHelper.GetMinDate();
                }
                if (companyPolicy.Endorsement.CurrentFrom <  DateHelper.GetMinDate())
                {
                    companyPolicy.Endorsement.CurrentFrom =  DateHelper.GetMinDate();
                }

                CompanyPolicy policy = DelegateService.TransportRenewalService.CreateRenewal(companyPolicy);
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
                var makeEndrsement = DelegateService.transportApplicationService.CanMakeEndorsement(policyId, out resulValidation);
                resulValidation.Add("CanMakeEndorsement", makeEndrsement);
                return new UifJsonResult(true, resulValidation);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, false);
            }
        }
    }
}