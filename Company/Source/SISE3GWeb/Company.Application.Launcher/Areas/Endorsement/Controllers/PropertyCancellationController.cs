using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PRM = Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyCancellationController : CancellationController
    {
        public ActionResult CreateTemporal(CancellationViewModel cancelationViewModel)
        {
            if (ModelState.IsValid)
            {
                
                CompanyPolicy policy;
                try
                {
                    cancelationViewModel.UserId = SessionHelper.GetUserId();
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(cancelationViewModel);
                    if (CompanyEndorsement != null)
                    {
                        policy = DelegateService.propertyCancellationServiceCia.CreateTemporalEndorsementCancellation(CompanyEndorsement);
                        if (policy != null)
                        {
                            return new UifJsonResult(true, policy);
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                        }
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                    }
                }
                catch (Exception)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
            else
            {
                string errorMessage = GetErrorMessages();
                return new UifJsonResult(false, errorMessage);
            }
            
        }

        public void ExecuteThread(List<PRM.CompanyPropertyRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                var policyExcute = DelegateService.propertyCancellationServiceCia.ExecuteThread(risksThread, policy, risks);
            }
            catch (Exception)
            {

                new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
    }
}