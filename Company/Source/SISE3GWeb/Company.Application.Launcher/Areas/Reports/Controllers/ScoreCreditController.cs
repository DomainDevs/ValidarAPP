using Sistran.Company.Application.ModelServices.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CPEMV1 = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reports.Controllers
{
    public class ScoreCreditController : Controller
    {
        // GET: Reports/ScoreCredit
        public ActionResult ScoreCredit()
        {
            return View();
        }


        public ActionResult GetDocumentType(int typeDocument)
        {
            try
            {
                List<CPEMV1.DocumentType> documentTypes = DelegateService.uniquePersonServiceV1.GetDocumentTypes(typeDocument);
                return new Web.Models.UifJsonResult(true, documentTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new Web.Models.UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetHistoryScoreCredit(int documentType, string documentNum)
        {
            try
            {
                ScoreCreditsServiceModel scoreCreditsServiceModel = new ScoreCreditsServiceModel();
                scoreCreditsServiceModel = null;//DelegateService.reportsService.GetScoredCredit(documentType, documentNum);
                
                return new UifSelectResult(scoreCreditsServiceModel);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}