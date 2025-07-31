using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Externals.Controllers
{
    public class CexperController : Controller
    {

        public ActionResult Cexper()
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
                return new UifJsonResult(false, Language.MSGWRN_CAPTCHA);
            }
        }

        public ActionResult GetDocumentType(int typeDocument)
        {
            try
            {
                List<DocumentType> documentTypes = DelegateService.uniquePersonServiceV1.GetDocumentTypes(typeDocument);
                return new UifJsonResult(true, documentTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetCexper(string licensePlate, int documentType, string documentNumber)
        {
            RequestCexper request = new RequestCexper() { Plate = licensePlate, DocumentType = documentType, DocumentNumber = documentNumber };
            ResponseCexper response = DelegateService.ExternalServiceWeb.ExecuteWebServiceCEXPERQueryBiztalk(request);
            if (response == null)
            {
                return new UifJsonResult(false, Language.ErrorServiceFall);
            }
            else if (response.PoliciesInfo == null || (response.PoliciesInfo.Count == 0 && response.SinisterInfo.Count == 0))
            {
                return new UifJsonResult(false, Language.ErrorServiceNull);
            }
            else if (response.PoliciesInfo != null || response.SinisterInfo != null) 
            {
                for (int i = 0; i < response.PoliciesInfo.Count; i++)
                {
                    response.PoliciesInfo[i].InsuredTypeDocument = Regex.Replace(response.PoliciesInfo[i].InsuredTypeDocument, @"[^a-zA-z0-9 ]", "E");
                }
                return new UifJsonResult(true, response);
            }
            else
            {
                return new UifJsonResult(false, Language.ErrorServiceNull);
            }
        }


        public ActionResult GetTypeCrossing(ResponseGoodExperienceYear response)
        {
            var ltstypeCrossing = response.PoliciesHistorical.Select(x => x.TypeCrossing).Distinct().ToList();
            List<TypeCrossing> loadTypes = new List<TypeCrossing>();

            if (ltstypeCrossing != null)
            {
                foreach (string item in ltstypeCrossing)
                {
                    int id = 0;
                    //int.TryParse(item.ToList().ToString(), out id);
                    loadTypes.Add(new TypeCrossing()
                    {
                        Id = id,
                        Description = item
                    });
                }
            }
            return new UifJsonResult(true, loadTypes);
        }

        public ActionResult GetTypeCrossingSinister(ResponseGoodExperienceYear response)
        {
            var ltsTypeCrossingSinister = response.HistoricalSinister.Select(x => x.TypeCrossing).Distinct().ToList();
            List<TypeCrossing> loadTypes = new List<TypeCrossing>();
            if (ltsTypeCrossingSinister != null)
            {
                foreach (string item in ltsTypeCrossingSinister)
                {
                    int id = 0;
                    //int.TryParse(item.ToList().ToString(), out id);
                    loadTypes.Add(new TypeCrossing()
                    {
                        Id = id,
                        Description = item
                    });
                }
            }
            return new UifJsonResult(true, loadTypes);
        }


        internal class TypeCrossing
        {
            /// <summary>
            /// Obtiene o establece el id
            /// </summary>
            public int Id { get; set; }


            /// <summary>
            /// Obtiene o establece la descripcion del tipo de cruce
            /// </summary>
            public string Description { get; set; }
        }
    }
}