using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class TransportExtensionController : ExtensionController
    {
        public ActionResult CreateTemporal(ExtensionViewModel extensionModel)
        {
            if (extensionModel != null)
            {
                try
                {
                    if (extensionModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                        ModelState["extensionModel.BusinessTypeDescription"].Errors.Clear();
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(extensionModel);
                    if (CompanyEndorsement != null)
                    {
                        var policy = DelegateService.transportExtensionService.CreateExtension(CompanyEndorsement);
                        if (policy != null)
                        {

                            // se Cambia a policy.Endorsement.Id que es quien trae el valor Identity del reverso Endorsement creado
                            var policyWF = DelegateService.ReversionEndorsementService.CreateEndorsementWorkFlow(extensionModel.PolicyId, policy.Endorsement.Id, extensionModel.TicketNumber.ToString(), Convert.ToDateTime(extensionModel.TicketDate));
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
                catch (Exception ex)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
            else
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

        //public ActionResult GetInsuredObjectsByRiskId(int riskId)
        //{
        //    try
        //    {
        //        List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();
        //        insuredObjects = DelegateService.TransportBusinessService.GetInsuredObjectsByRiskId(riskId);
        //        return new UifJsonResult(true, insuredObjects.Where(x => x.IsDeclarative == true));
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredObject);
        //    }
        //}
    }
}