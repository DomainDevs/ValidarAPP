namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.RulesScriptsServices.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifSelect;
    using Enums = Sistran.Core.Application.RulesScriptsServices.Enums;

    [Authorize]
    public class ScriptsController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult AdvancedSearchScript()
        {
            return View();
        }

        public ViewResult GuionAdd()
        {
            return View();
        }

        #region datos

        public JsonResult GetScriptsAutocomplete(string query)
        {
            List<Script> scripts = DelegateService.scriptsService.GetScripts().Where(x => x.Description.ToLower().Contains(query.ToLower())).ToList();
            return Json(scripts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionsAutocomplete(string query)
        {
            List<Question> scripts = DelegateService.scriptsService.GetQuestions().Where(x => x.Description.ToLower().Contains(query.ToLower())).ToList();
            return Json(scripts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScripts()
        {
            try
            {
                List<Script> scripts = DelegateService.scriptsService.GetScripts();
                return Json(scripts, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult GetScriptByLevelId(int? module, int? level, string Name, string Question)
        {
            try
            {
                List<Script> scripts = DelegateService.scriptsService.GetScriptByLevelId(module, level, Name, Question);
                return Json(scripts, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult GetQuestionsControlByLevel(int level, int module)
        {
            try
            {
                List<Question> questions = new List<Question>();
                if (module != 21)
                {
                    questions = DelegateService.scriptsService.GetQuestionsByLevel((Enums.Level)level);
                }
                else if (module == 21)
                {
                    questions = DelegateService.scriptsService.GetQuestionsByLevelAutomaticQuota((Enums.LevelAutomaticQuota)level);
                }
                return new UifSelectResult(questions);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }

        public JsonResult GetQuestionControl(int QuestionId)
        {
            try
            {
                Question question = DelegateService.scriptsService.GetQuestion(QuestionId);

                return Json(question, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }

        public JsonResult GetPackages()
        {
            try
            {
                List<Package> packages = DelegateService.rulesEditorServices.GetPackages().Where(p => p.Disabled == false).ToList();
                return new UifSelectResult(packages.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Package>());
            }
        }

        public JsonResult GetLevels(int? packageId)
        {
            try
            {
                List<Level> levels = DelegateService.rulesEditorServices.GetLevels((int)packageId);
                return new UifSelectResult(levels.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Level>());
            }
        }

        public JsonResult GetScriptComposite(int ScriptId)
        {
            try
            {
                ScriptComposite ScriptComposite = DelegateService.scriptsService.GetScriptComposite(ScriptId);
                return Json(ScriptComposite, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult SaveQuestion(ScriptComposite ScriptComposite)
        {
            try
            {
                if (ScriptComposite != null)
                {
                    ScriptComposite = DelegateService.scriptsService.CreateScriptComposite(ScriptComposite);
                }

                return Json(ScriptComposite, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult DeleteScript(int ScriptId)
        {
            try
            {
                DelegateService.scriptsService.DeleteScript(ScriptId);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        #endregion

        /// <summary>
        /// Genera archivo excel de guines
        /// </summary>
        /// <returns>Url del archivo</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                string urlFile = DelegateService.scriptsService.GenerateScriptsReport(App_GlobalResources.Language.LabelScript + " - " + DateTime.Now.ToString("dd-MM-yyyy"));
                if (!string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}