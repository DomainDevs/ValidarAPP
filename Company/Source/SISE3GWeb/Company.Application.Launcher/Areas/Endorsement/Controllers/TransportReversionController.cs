using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class TransportReversionController : ReversionController
    {
        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public ActionResult CreateTemporal(ReversionViewModel reversionModel)
        {
            
            if (reversionModel != null)
            {
                try
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                    if (CompanyEndorsement != null)
                    {
                        //var policy = DelegateService.propertyReversionEndorsementCia.CreateEndorsementReversion(CompanyEndorsement);
                        var policy = DelegateService.ITransportReversionService.CreateEndorsementReversion(CompanyEndorsement, false);

                        if (policy != null)
                        {
                            // se Cambia a policy.Endorsement.Id que es quien trae el valor Identity del reverso Endorsement creado
                            var policyWF = DelegateService.ReversionEndorsementService.CreateEndorsementWorkFlow(reversionModel.PolicyId, policy.Endorsement.Id, reversionModel.TicketNumber.ToString(), Convert.ToDateTime(reversionModel.TicketDate));

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
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
    }
}