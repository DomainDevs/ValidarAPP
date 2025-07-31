using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Externals.Controllers
{
    public class FasecoldaSISAController : Controller
    {
        // GET: Externals/FasecoldaSISA
        public ActionResult FasecoldaSISA()
        {
            return View();
        }

        public ActionResult GetTextCaptcha()
        {
            if (Session["Captcha"] != null)
            {
                return new UifJsonResult(true, Session["Captcha"].ToString());
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MSGWRN_CAPTCHA);
            }
        }

        public ActionResult GetFasecoldaInfo(string plate, string engine, string chassis)
        {
            RequestFasecoldaSISA request = new RequestFasecoldaSISA() { Plate = plate, Engine = engine, Chassis = chassis };
            ResponseFasecoldaSISA response = DelegateService.ExternalServiceWeb.ExecuteWebServiceSISAQueryBiztalk(request);

            if (response == null)
            {
                return new UifJsonResult(false, Language.ErrorServiceFall);
            }
            else if(response.GuideInfo != null )
            {
                return new UifJsonResult(true, response);
            }
            else
            {
                return new UifJsonResult(false, Language.ErrorServiceNull);
            }
        }

        public ActionResult SIMIT()
        {
            return View();
        }

        public ActionResult GetFasecoldaSIMIT(string documentType, string documentNumber)
        {
            //ResponseInfringement response = DelegateService.ExternalServiceWeb.ExecuteWebServiceBizTalkInfringementSimit(/*documentType*/1, documentNumber, new Guid());
            ResponseInfringement response = new ResponseInfringement()
            {
                ExternalInfrigements = new List<ExternalInfrigement>()
                {
                    new ExternalInfrigement()
                    {
                        Code = "1",
                        DateRequest = new DateTime(),
                        InfringementDate = new DateTime(),
                        InfringementState = "Acitvo",
                        LicensePlate = "Placa",
                        Number = "Número",
                        Price = 10
                    },
                    new ExternalInfrigement()
                    {
                        Code = "2",
                        DateRequest = new DateTime(),
                        InfringementDate = new DateTime(),
                        InfringementState = "Inacitvo",
                        LicensePlate = "Placa",
                        Number = "Número",
                        Price = 20
                    }
                }
            };
            return new UifJsonResult(true, response);
        }

        public ActionResult GetDocumentTypes()
        {
            try
            {
                List<DocumentType> documentTypes = DelegateService.uniquePersonServiceV1.GetDocumentTypes(1);
                return new UifJsonResult(true, documentTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetDocumentTypes);
            }

        }
    }
}