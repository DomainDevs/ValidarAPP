using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class LiabilityCancellationController : CancellationController
    {


        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {

            try
            {
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                CompanyEndorsement.OnlyCancelation = true;
                if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                    CompanyEndorsement.Text.TextBody = unicode_iso8859(CompanyEndorsement.Text.TextBody);
                var policy = DelegateService.liabilityCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, e.GetBaseException().Message);
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