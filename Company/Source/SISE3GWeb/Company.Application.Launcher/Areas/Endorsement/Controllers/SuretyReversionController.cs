using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyReversionController : ReversionController
    {
        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public ActionResult CreateTemporal(ReversionViewModel reversionModel)
        {
            try
            {
                if (reversionModel == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
                else
                {
                    int UserId = Helpers.SessionHelper.GetUserId();
                    reversionModel.UserId = UserId;
                    CompanyEndorsement companyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                    CompanyPolicy companyPolicy = DelegateService.suretyReversionEndorsement.CreateEndorsementReversion(companyEndorsement, false);

                    return new UifJsonResult(true, companyPolicy);
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
    }
}