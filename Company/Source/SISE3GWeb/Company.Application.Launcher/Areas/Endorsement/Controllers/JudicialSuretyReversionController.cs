using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class JudicialSuretyReversionController : ReversionController
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
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                var policy = DelegateService.JudicialSuretyReversionService.CreateEndorsementReversion(CompanyEndorsement, false);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
    }
}