using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Controllers
{
    public class MatchingProcessController : Controller
    {
        public ActionResult MatchingProcess()
        {
            return View();
        }

        public JsonResult GetPersonRiskList(string SearchValue)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonListRiskApplication.GenerateListRiskProcessRequest(true, false, false, SearchValue, false));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
                throw;
            }
        }

        public JsonResult StartCompleteProcess()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonListRiskApplication.GenerateListRiskProcessRequest(true, false, false, "", true));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
                throw;
            }
        }

        public JsonResult SearchProcess(string SearchValue)
        {
            try
            {
                ListRiskMatchDTO listRiskProcess = DelegateService.uniquePersonListRiskApplication.SearchProcess(SearchValue);
                if (listRiskProcess.fileName != null)
                {
                    listRiskProcess.fileName = listRiskProcess.fileName.Replace("archivoexcel", "archvosCoincidencias");
                }
                
                return new UifJsonResult(true, listRiskProcess);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
                throw;
            }
        }
    }
}