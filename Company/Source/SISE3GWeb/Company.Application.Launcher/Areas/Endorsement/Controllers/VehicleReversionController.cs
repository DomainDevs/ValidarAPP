using System;
using System.Web.Mvc;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;
using System.Threading;
using System.Collections.Generic;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Newtonsoft.Json;
using System.Linq;
using Sistran.Company.Application.UnderwritingServices.Models;
using AutoMapper;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using TypePolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.UIF.Web.Controllers;
using System.Text.RegularExpressions;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class VehicleReversionController : ReversionController
    {

        public ActionResult CreateTemporal(ReversionViewModel reversionModel)
        {
            try
            {
                if (reversionModel == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
                int id = Convert.ToInt32(reversionModel.EndorsementId);
                int UserId = SessionHelper.GetUserId();

                if (DelegateService.utilitiesServiceCore.GetEndorsementControlById(id, UserId))
                {
                    reversionModel.UserId = UserId;
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                    CompanyEndorsement.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);

                    if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                        CompanyEndorsement.Text.TextBody = unicode_iso8859(CompanyEndorsement.Text.TextBody);
                    var policy = DelegateService.vehicleReversionServiceCia.CreateEndorsementReversion(CompanyEndorsement, false);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyInEdition);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
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